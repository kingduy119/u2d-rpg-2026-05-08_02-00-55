using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using Weapons;

public interface IPoolable<T> where T : MonoBehaviour
{
    IObjectPool<T> Pool { get; set; }
}

public class Factory<Type, T> where T : MonoBehaviour,
    IPoolable<T>
{
    protected Dictionary<Type, GameObject> prefabs = new();
    protected Dictionary<Type, GenericPool<T>> _PoolDictionary = new();
    private GameObject _poolParent;

    public string Name { get; private set; }
    public Factory(string name) { Name = name; }

    public GameObject PoolParent
    {
        get
        {
            if (_poolParent == null) _poolParent = new(Name);
            return _poolParent;
        }
    }

    public void AddPrefab(Type type, T prefab)
    {
        if (!prefabs.ContainsKey(type))
        {
            prefabs.Add(type, prefab.gameObject);
        }
    }

    public T GetObject(Type type)
    {
        if (type == null) return null;

        if (!prefabs.TryGetValue(type, out var prefab))
            return null;

        if (!_PoolDictionary.TryGetValue(type, out var pool))
        {
            pool = new GenericPool<T>(prefab, PoolParent.transform);
            _PoolDictionary.Add(type, pool);
        }
        return pool.Get();
    }

    public void ClearPool()
    {
        foreach (var pool in _PoolDictionary.Values)
            pool.Clear();

        _PoolDictionary.Clear();

        if (_poolParent != null)
            Object.Destroy(_poolParent.gameObject);

        _poolParent = null;
    }
}

public class GenericPool<T> where T : MonoBehaviour,
    IPoolable<T>
{
    private readonly IObjectPool<T> _pool;

    public GenericPool(
        GameObject prefab,
        Transform parent,
        int defaultCapacity = 10,
        int maxSize = 100)
    {
        _pool = new ObjectPool<T>(
            () =>
            {
                GameObject go = Object.Instantiate(prefab, parent);

                T instance = go.GetComponent<T>();
                instance.Pool = _pool;

                return instance;
            },
            OnGet,
            OnRelease,
            OnDestroy,
            true,
            defaultCapacity,
            maxSize);
    }

    private void OnGet(T item)
    {
        if (item) item.gameObject.SetActive(true);
    }
    private void OnRelease(T item)
    {
        if (item) item.gameObject.SetActive(false);
    }
    private void OnDestroy(T item)
    {
        if (item != null && item.gameObject != null) Object.Destroy(item.gameObject);
    }
    public T Get() => _pool.Get();
    public void Clear() => _pool.Clear();
}


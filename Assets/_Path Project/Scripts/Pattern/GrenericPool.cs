using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class GenericPool<T> where T : Component,
    IPoolable<T>
    {
        private readonly IObjectPool<T> _pool;
        private HashSet<T> _activeObjects = new HashSet<T>();

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
            _activeObjects.Add(item);
            item.gameObject.SetActive(true);
        }
        private void OnRelease(T item)
        {
            _activeObjects.Remove(item);
            item.gameObject.SetActive(false);
        }
        private void OnDestroy(T item)
        {
            if (item != null)
            {
                Object.Destroy(item.gameObject);
            }
        }
        public T Get() => _pool.Get();
    }
}

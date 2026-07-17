using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class GenericPool<T> where T : Component,
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
                obj => obj.gameObject.SetActive(true),
                obj => obj.gameObject.SetActive(false),
                obj => Object.Destroy(obj.gameObject),
                true,
                defaultCapacity,
                maxSize);
        }

        public T Get()
        {
            return _pool.Get();
        }

        public void Release(T obj)
        {
            _pool.Release(obj);
        }

        public IObjectPool<T> Pool => _pool;
    }
}

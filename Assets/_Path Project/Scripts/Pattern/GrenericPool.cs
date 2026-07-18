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
                OnGet,
                OnRelease,
                OnDestroy,
                true,
                defaultCapacity,
                maxSize);
        }

        private void OnGet(T item) => item.gameObject.SetActive(true);
        private void OnRelease(T item) => item.gameObject.SetActive(false);
        private void OnDestroy(T item) => Object.Destroy(item.gameObject);

        public T Get() => _pool.Get();
    }
}

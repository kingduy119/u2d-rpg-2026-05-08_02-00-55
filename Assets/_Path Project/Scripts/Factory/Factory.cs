using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{
    public abstract class Factory<Type, T> : MonoBehaviour
        where T : MonoBehaviour, IPoolable<T>
    {
        private readonly Dictionary<Type, GenericPool<T>> _PoolDictionary = new();
        [Serializable]
        private class Config
        {
            public Type type;
            public GameObject prefab;
        }

        [SerializeField] private List<Config> _configs;
        private readonly Dictionary<Type, Config> _configMap = new();

        private void Awake()
        {
            foreach (var config in _configs)
            {
                _configMap.Add(config.type, config);
            }
        }

        public T GetObject(Type type)
        {
            if (!_configMap.TryGetValue(type, out var config))
                return null;

            if (!_PoolDictionary.TryGetValue(type, out var pool))
            {
                pool = CreatePool(config);
                _PoolDictionary.Add(type, pool);
            }

            return pool.Get();
        }


        private GenericPool<T> CreatePool(Config config)
        {
            GameObject prefab = config.prefab;
            var pool = new GenericPool<T>(prefab, transform);
            return pool;
        }
    }

    public abstract class NewFactory<Type, T> where T : Component,
        IPoolable<T>
    {
        protected readonly Dictionary<Type, GameObject> prefabs = new();
        protected readonly Dictionary<Type, GenericPool<T>> _PoolDictionary = new();
        protected AsyncOperationHandle<IList<GameObject>> handle;
        protected List<string> loadKeys;


        public async void LoadPrefabs()
        {
            handle = Addressables.LoadAssetsAsync<GameObject>(loadKeys, null, Addressables.MergeMode.Union);
            await handle.Task;

            handle.Completed += OnLoadCompelete;
        }

        private void OnLoadCompelete(AsyncOperationHandle<IList<GameObject>> asyncHandle)
        {
            if (asyncHandle.Status == AsyncOperationStatus.Succeeded)
            {
                IList<GameObject> results = asyncHandle.Result;
                for (int i = 0; i < results.Count; i++)
                {
                    MapGameObject(results[i]);
                }

            }
        }

        protected abstract void MapGameObject(GameObject go);

        public T GetObject(Type type, Transform transform)
        {
            if (!prefabs.TryGetValue(type, out var prefab))
                return null;

            if (!_PoolDictionary.TryGetValue(type, out var pool))
            {
                pool = new GenericPool<T>(prefab, transform);
                _PoolDictionary.Add(type, pool);
            }
            return pool.Get();
        }

        public void Destroy()
        {
            if (handle.IsValid())
            {
                Debug.Log("OnDestroy.handle.IsValid");
                handle.Completed -= OnLoadCompelete;
                Addressables.Release(handle);
            }
        }
    }
}
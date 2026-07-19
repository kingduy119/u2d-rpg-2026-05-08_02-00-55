using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public abstract class BaseFactory<Type, T> : MonoBehaviour
        where T : MonoBehaviour, IPoolable<T>
    {
        private readonly Dictionary<Type, GenericPool<T>> _pool = new();
        [Serializable]
        private class Config
        {
            public Type type;
            public GameObject prefab;
        }
        [SerializeField] private List<Config> _configs;
        private Dictionary<Type, Config> _configMap = new();


        private void Awake()
        {
            foreach (var config in _configs)
            {
                _configMap.Add(config.type, config);
            }
        }

        private GenericPool<T> CreatePool(Config config)
        {
            GameObject prefab = config.prefab;
            var pool = new GenericPool<T>(prefab, transform);
            return pool;
        }

        public T GetObject(Type type)
        {
            if (!_configMap.TryGetValue(type, out var config))
                return null;

            if (!_pool.TryGetValue(type, out var pool))
            {
                pool = CreatePool(config);
                _pool.Add(type, pool);
            }

            return pool.Get();
        }
    }
}
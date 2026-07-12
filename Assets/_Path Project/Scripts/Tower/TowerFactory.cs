using UnityEngine;
using UnityEngine.Pool;
using System;
using System.Collections.Generic;

namespace TDGame
{
    public class TowerFactory : MonoBehaviour
    {
        private readonly Dictionary<TowerType, GenericPool<TowerBase>> _pool = new();


        [Serializable]
        private class Config
        {
            public TowerType type;
            public GameObject prefab;
        }
        [SerializeField] private List<Config> _configs;
        private Dictionary<TowerType, Config> _configMap = new();


        private void Awake()
        {
            foreach (var config in _configs)
            {
                _configMap.Add(config.type, config);
            }
        }

        private GenericPool<TowerBase> CreatePool(Config config)
        {
            GameObject prefab = config.prefab;
            var poolNew = new GenericPool<TowerBase>(prefab, this.transform);
            return poolNew;
        }

        public TowerBase GetObject(TowerType type)
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


using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{

    public abstract class NewFactory<ScriptableObject, T> : MonoBehaviour
        where T : MonoBehaviour, IPoolable<T>
    {
        private readonly Dictionary<ScriptableObject, GenericPool<T>> _PoolDictionary = new();
        [Serializable]
        private class Config
        {
            public ScriptableObject type;
            public GameObject prefab;
        }
        [SerializeField] private List<Config> _configs;
        private Dictionary<ScriptableObject, Config> _configMap = new();


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

        public T GetObject(ScriptableObject SO)
        {
            if (!_configMap.TryGetValue(SO, out var config))
                return null;

            if (!_PoolDictionary.TryGetValue(SO, out var pool))
            {
                pool = CreatePool(config);
                _PoolDictionary.Add(SO, pool);
            }

            return pool.Get();
        }
    }
    public class TestFactory : NewFactory<TowerSO, TowerBase>
    {

    }

    public class TowerFactory2 : Factory<TowerSO, TowerBase> { }
}
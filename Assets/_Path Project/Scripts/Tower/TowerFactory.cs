using UnityEngine;
using UnityEngine.Pool;
using System;
using System.Collections.Generic;

namespace TDGame
{
    public class TowerFactory : MonoBehaviour
    {
        [Serializable]
        public struct TowerConfig
        {
            public TowerType m_type;
            public GameObject m_prefab;
        }

        [SerializeField] private int defaultCapacity = 3;
        [SerializeField] private int maxPoolSize = 100;
        [SerializeField] private List<TowerConfig> towerConfigs;

        private Dictionary<TowerType, IObjectPool<TowerBase>> m_pool = new();

        private Dictionary<TowerType, TowerConfig> configDictionary = new();

        private void Awake()
        {
            foreach (var config in towerConfigs)
            {
                configDictionary.Add(config.m_type, config);

                IObjectPool<TowerBase> pool = InitPool(config);
                m_pool.Add(config.m_type, pool);
            }
        }

        private ObjectPool<TowerBase> InitPool(TowerConfig config)
        {
            GameObject prefab = configDictionary[config.m_type].m_prefab;

            ObjectPool<TowerBase> pool = null;
            pool = new ObjectPool<TowerBase>(
                () => CreateTower(config.m_type),
                go => go.gameObject.SetActive(true),
                go => go.gameObject.SetActive(false),
                go => Destroy(go.gameObject),
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxPoolSize
            );
            return pool;
        }

        private TowerBase CreateTower(TowerType type)
        {
            GameObject prefab = configDictionary[type].m_prefab;
            GameObject instanceGo = Instantiate(prefab, this.transform);

            TowerBase tower = instanceGo.GetComponent<TowerBase>();

            if (tower == null)
                return null;

            tower.Pool = m_pool[type];

            return tower;
        }

        public TowerBase GetObject(TowerType type)
        {
            if (!m_pool.TryGetValue(type, out var pool))
            {
                Debug.LogError($"No pool found for {type}");
                return null;
            }

            return pool.Get();
        }
    }
}
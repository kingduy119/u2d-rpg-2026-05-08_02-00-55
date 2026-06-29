using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class ProjectileFactory : PersistentSingleton<ProjectileFactory>
    {
        [Serializable]
        public struct ProjectileConfig
        {
            public ProjectileType type;
            public GameObject prefab;
            public int defaultCapacity;
            public int maxPoolSize;
        }

        [SerializeField] private List<ProjectileConfig> projectileConfigs;

        private Dictionary<ProjectileType, IObjectPool<Projectile>> m_pool = new();

        private Dictionary<ProjectileType, ProjectileConfig> configDictionary = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (var config in projectileConfigs)
            {
                configDictionary.Add(config.type, config);

                IObjectPool<Projectile> pool = InitPool(config);
                m_pool.Add(config.type, pool);
            }
        }

        private ObjectPool<Projectile> InitPool(ProjectileConfig config)
        {
            GameObject prefab = configDictionary[config.type].prefab;

            ObjectPool<Projectile> pool = null;
            pool = new ObjectPool<Projectile>(
                () => CreateProjectile(config.type),
                go => go.gameObject.SetActive(true),
                go => go.gameObject.SetActive(false),
                go => Destroy(go.gameObject),
                collectionCheck: true,
                    defaultCapacity: config.defaultCapacity,
                    maxSize: config.maxPoolSize
            );
            return pool;
        }

        private Projectile CreateProjectile(ProjectileType type)
        {
            GameObject prefab = configDictionary[type].prefab;
            GameObject instanceGo = Instantiate(prefab, this.transform);

            Projectile projectile = instanceGo.GetComponent<Projectile>();

            if (projectile == null)
                return null;

            projectile.Pool = m_pool[type];

            return projectile;
        }

        public Projectile GetObject(ProjectileType type)
        {
            if (!m_pool.TryGetValue(type, out var pool))
            {
                Debug.LogError($"No pool found for {type}");
                return null;
            }

            return pool.Get();
        }


    } // Class
}
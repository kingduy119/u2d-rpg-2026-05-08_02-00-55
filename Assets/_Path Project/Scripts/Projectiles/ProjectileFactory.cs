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

        private Dictionary<ProjectileType, IObjectPool<ProjectileBase>> m_pool = new();

        private Dictionary<ProjectileType, ProjectileConfig> configDictionary = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (var config in projectileConfigs)
            {
                configDictionary.Add(config.type, config);

                // IObjectPool<ProjectileBase> pool = new ObjectPool<ProjectileBase>(
                //     createFunc: () => CreateProjectile(config.type),
                //     actionOnGet: (proj) => proj.gameObject.SetActive(true),
                //     actionOnRelease: (proj) => proj.gameObject.SetActive(false),
                //     actionOnDestroy: (proj) => Destroy(proj.gameObject),
                //     collectionCheck: true,
                //     defaultCapacity: config.defaultCapacity,
                //     maxSize: config.maxPoolSize
                // );
                IObjectPool<ProjectileBase> pool = InitPool(config);
                m_pool.Add(config.type, pool);
            }
        }

        private ObjectPool<ProjectileBase> InitPool(ProjectileConfig config)
        {
            GameObject prefab = configDictionary[config.type].prefab;

            ObjectPool<ProjectileBase> pool = null;
            pool = new ObjectPool<ProjectileBase>(
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

        private ProjectileBase CreateProjectile(ProjectileType type)
        {
            GameObject prefab = configDictionary[type].prefab;
            GameObject instanceGo = Instantiate(prefab, this.transform);

            ProjectileBase projectile = instanceGo.GetComponent<ProjectileBase>();

            if (projectile == null)
            {
                Debug.LogError($"Prefab của {type} chưa gắn script implement IProjectile!");
                return null;
            }

            if (projectile is Arrow arrow)
            {
                arrow.Pool = m_pool[type];
            }
            // else if (projectile is MissileProjectile missile) { missile.SetPool(...); }

            return projectile;
        }

        public ProjectileBase GetObject(ProjectileType type)
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
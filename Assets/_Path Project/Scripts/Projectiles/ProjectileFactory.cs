using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class ProjectileFactory : MonoBehaviour
    {
        private readonly Dictionary<ProjectileType, IObjectPool<Projectile>> _pool = new();
        private readonly Dictionary<ProjectileType, GenericPool<Projectile>> _poolNew = new();


        [Serializable]
        private class Config
        {
            public ProjectileType type;
            public GameObject prefab;
        }
        [SerializeField] private List<Config> _configs;
        private Dictionary<ProjectileType, Config> _configMap = new();


        private void Awake()
        {
            foreach (var config in _configs)
            {
                _configMap.Add(config.type, config);
            }
        }

        // private IObjectPool<Projectile> InitPool(Config config)
        // {
        //     GameObject prefab = config.prefab;

        //     ObjectPool<Projectile> pool = null;
        //     pool = new ObjectPool<Projectile>(
        //         () =>
        //         {
        //             GameObject go = Instantiate(prefab, this.transform);

        //             if (!go.TryGetComponent<Projectile>(out var projectile))
        //                 return null;

        //             projectile.Pool = pool;
        //             return projectile;
        //         },
        //         go => go.gameObject.SetActive(true),
        //         go => go.gameObject.SetActive(false),
        //         go => Destroy(go.gameObject),
        //         collectionCheck: true,
        //         defaultCapacity: 3,
        //         maxSize: 100
        //     );
        //     return pool;
        // }

        private GenericPool<Projectile> CreatePool(Config config)
        {
            GameObject prefab = config.prefab;
            var poolNew = new GenericPool<Projectile>(prefab, this.transform);
            return poolNew;
        }

        public Projectile Get(ProjectileType type)
        {
            if (!_configMap.TryGetValue(type, out var config))
            {
                return null;
            }

            if (!_poolNew.TryGetValue(type, out var pool))
            {
                pool = CreatePool(config);
                _poolNew.Add(type, pool);
            }

            return pool.Get();
        }

        // public Projectile GetObject(ProjectileType type)
        // {

        //     if (!_configMap.TryGetValue(type, out var config))
        //     {
        //         return null;
        //     }

        //     if (!_pool.TryGetValue(type, out var pool))
        //     {
        //         pool = InitPool(_configs.Find(c => c.type == type));
        //         _pool.Add(type, pool);
        //     }

        //     return pool.Get();
        // }
    }

}
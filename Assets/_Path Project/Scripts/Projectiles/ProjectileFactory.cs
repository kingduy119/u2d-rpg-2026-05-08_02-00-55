using System;
using System.Collections.Generic;
using UnityEngine;


namespace TDGame
{
    public class ProjectileFactory : MonoBehaviour
    {
        private readonly Dictionary<ProjectileType, GenericPool<Projectile>> _pool = new();

        [Serializable]
        private class Config
        {
            public ProjectileType type;
            public GameObject prefab;
        }
        [SerializeField] private List<Config> _configs;
        private readonly Dictionary<ProjectileType, Config> _configMap = new();


        private void Awake()
        {
            foreach (var config in _configs)
            {
                _configMap.Add(config.type, config);
            }
        }

        private GenericPool<Projectile> CreatePool(Config config)
        {
            GameObject prefab = config.prefab;
            var poolNew = new GenericPool<Projectile>(prefab, transform);
            return poolNew;
        }

        public Projectile GetObject(ProjectileType type)
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
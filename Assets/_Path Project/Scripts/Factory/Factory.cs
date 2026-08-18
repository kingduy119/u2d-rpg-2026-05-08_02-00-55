using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public abstract class OldFactory<Type, T> : MonoBehaviour
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

    public abstract class Factory<Type, T> where T : Component,
        IPoolable<T>
    {
        protected Dictionary<Type, GameObject> prefabs = new();
        protected Dictionary<Type, GenericPool<T>> _PoolDictionary = new();

        public abstract void AddPrefab(T prefab);

        public T GetObject(Type type, Transform transform)
        {
            if (!prefabs.TryGetValue(type, out var prefab))
                return null;

            // Create new pool with type if not exists
            if (!_PoolDictionary.TryGetValue(type, out var pool))
            {
                pool = new GenericPool<T>(prefab, transform);
                _PoolDictionary.Add(type, pool);
            }
            return pool.Get();
        }
    }

    public class TowerFactory : Factory<TowerSO, Tower>
    {
        public override void AddPrefab(Tower entity)
        {
            if (!prefabs.ContainsKey(entity.SO))
            {
                prefabs.Add(entity.SO, entity.gameObject);
            }
        }
    }

    public class EnemyFactory : Factory<EnemySO, Enemy>
    {
        public override void AddPrefab(Enemy entity)
        {
            if (!prefabs.ContainsKey(entity.SO))
            {
                prefabs.Add(entity.SO, entity.gameObject);
            }
        }
    }

    public class ProjectileFactory : Factory<ProjectileSO, Projectile>
    {
        public override void AddPrefab(Projectile entity)
        {
            if (!prefabs.ContainsKey(entity.SO))
            {
                prefabs.Add(entity.SO, entity.gameObject);
            }
        }
    }
}
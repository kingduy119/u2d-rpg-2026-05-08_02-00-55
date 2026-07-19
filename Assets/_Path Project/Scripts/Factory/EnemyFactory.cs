using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{

    public class EnemyFactory : BaseFactory<EnemyType, Enemy>
    { }
    // : MonoBehaviour
    // {
    //     private readonly Dictionary<EnemyType, GenericPool<Enemy>> _pool = new();

    //     [Serializable]
    //     private class Config
    //     {
    //         public EnemyType type;
    //         public GameObject prefab;
    //     }
    //     [SerializeField] private List<Config> _configs;
    //     private Dictionary<EnemyType, Config> _configMap = new();


    //     private void Awake()
    //     {
    //         foreach (var config in _configs)
    //         {
    //             _configMap.Add(config.type, config);
    //         }
    //     }

    //     private GenericPool<Enemy> CreatePool(Config config)
    //     {
    //         GameObject prefab = config.prefab;
    //         var pool = new GenericPool<Enemy>(prefab, transform);
    //         return pool;
    //     }

    //     public Enemy GetObject(EnemyType type)
    //     {
    //         if (!_configMap.TryGetValue(type, out var config))
    //             return null;

    //         if (!_pool.TryGetValue(type, out var pool))
    //         {
    //             pool = CreatePool(config);
    //             _pool.Add(type, pool);
    //         }

    //         return pool.Get();
    //     }

    // }
}

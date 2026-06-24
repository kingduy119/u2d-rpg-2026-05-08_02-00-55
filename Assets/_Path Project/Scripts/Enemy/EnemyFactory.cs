using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace TDGame
{

    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private bool collectionCheck = true;
        [SerializeField] private int capacity = 20;
        [SerializeField] private int maxSize = 200;
        [SerializeField] private EnemyData[] enemyDatas;

        private Dictionary<EnemyType, ObjectPool<Enemy>> pools;

        private void Awake()
        {
            pools = new Dictionary<EnemyType, ObjectPool<Enemy>>();

            foreach (var data in enemyDatas)
            {
                var pool = InitPool(data.prefab);
                pools.Add(data.type, pool);
            }
        }

        private ObjectPool<Enemy> InitPool(Enemy prefab)
        {
            ObjectPool<Enemy> pool = null;
            pool = new ObjectPool<Enemy>(
                () =>
                {
                    Enemy enemy = Instantiate(prefab);
                    enemy.Pool = pool;
                    return enemy;
                },
                go => go.gameObject.SetActive(true),
                go => go.gameObject.SetActive(false),
                go => Destroy(go.gameObject),
                collectionCheck,
                capacity,
                maxSize
            );
            return pool;
        }


        public Enemy GetEnemy(EnemyType type)
        {
            if (!pools.TryGetValue(type, out var pool))
            {
                Debug.LogError($"No pool found for {type}");
                return null;
            }

            return pool.Get();
        }

    }
}

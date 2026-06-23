using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
            foreach (var data in enemyDatas)
            {
                var pool = new ObjectPool<Enemy>(
                    () => CreateEnemy(data.prefab),
                    OnGet,
            OnRelease,
            OnDestroyPoolObject,
            collectionCheck, capacity, maxSize
                );
            }
        }

        private Enemy CreateEnemy(Enemy prefab)
        {
            return Instantiate(prefab);
        }

        private void OnGet(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        private void OnRelease(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }

        public Enemy Create(EnemyData data)
        {
            Enemy enemy = pools[data.type].Get();

            // enemy.Initialize(data);

            return enemy;
        }
    }
}

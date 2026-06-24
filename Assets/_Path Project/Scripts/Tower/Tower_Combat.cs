using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class Tower_Combat : MonoBehaviour
    {
        [SerializeField] private TowerData _data;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private bool collectionCheck = true;
        [SerializeField] private int capacity = 20;
        [SerializeField] private int maxSize = 200;

        private float _shootTimer;


        private IObjectPool<Projectile> _objectPool;
        private CircleCollider2D _circleCollider;

        public List<Enemy> _enemiesInRange = new();


        private void Awake()
        {
            _objectPool = new ObjectPool<Projectile>(
                CreateProjectile,
                po => po.gameObject.SetActive(true),// OnGetFromPool, 
                po => po.gameObject.SetActive(false),// OnRealeaseToPool, 
                po => Destroy(po.gameObject),// OnDestroyPooledObject,
                collectionCheck, capacity, maxSize
            );
        }

        private void OnEnable()
        {
            Enemy.OnEnemyDestroyed += HandleEnemeyDestroyed;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyDestroyed -= HandleEnemeyDestroyed;
        }

        private void Start()
        {
            _circleCollider = GetComponent<CircleCollider2D>();
            _circleCollider.radius = _data.range;
            _enemiesInRange = new List<Enemy>();
            _shootTimer = _data.shootInterval;
        }

        private void Update()
        {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0)
            {
                _shootTimer = _data.shootInterval;
                Shoot();
            }
        }

        private Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(projectilePrefab);
            projectile.ObjectPool = _objectPool;
            return projectile;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent<Enemy>(out var enemy))
                {
                    _enemiesInRange.Add(enemy);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent<Enemy>(out var enemy))
                {
                    _enemiesInRange.Remove(enemy);
                }
            }
        }

        private void Shoot()
        {
            if (_enemiesInRange.Count > 0)
            {
                Projectile projectile = _objectPool.Get();
                if (projectile == null) return;

                Vector2 shootDirection = (_enemiesInRange[0].transform.position - transform.position).normalized;

                projectile.transform.position = transform.position;
                projectile.Shoot(_data, shootDirection);
            }
        }

        private void HandleEnemeyDestroyed(Enemy enemy)
        {
            _enemiesInRange.Remove(enemy);
        }
    }

}
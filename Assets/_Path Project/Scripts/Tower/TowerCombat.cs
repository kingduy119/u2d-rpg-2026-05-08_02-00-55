using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public class TowerCombat : MonoBehaviour
    {
        [SerializeField] private TowerBase Tower;
        private TowerSO m_TowerSO;
        private List<Enemy> m_enemiesInRange = new();
        private ProjectileFactory Factory => GameManager.Instance.FactoryManager.ProjectileFactory;
        private float m_shootTimer = 0f;

        [SerializeField] private GameObject _ProjectilePrefab;
        public GenericPool<Projectile> ProjectilePool;

        private void Awake()
        {
            m_enemiesInRange = new List<Enemy>();

            // if (TryGetComponent<TowerBase>(out var tower))
            // {
            //     Tower = tower;
            //     m_TowerSO = tower.TowerSO;
            // }
            if (Tower != null && TryGetComponent<CircleCollider2D>(out var collider))
            {
                m_TowerSO = Tower.TowerSO;
                collider.radius = Tower.TowerSO.ShootRange;
            }
        }

        private void OnEnable()
        {
            GameEvent.OnEnemyDie += HandleEnemyDie;
        }

        private void OnDisable()
        {
            GameEvent.OnEnemyDie -= HandleEnemyDie;
        }

        private void Update()
        {
            m_shootTimer -= Time.deltaTime;
            if (m_shootTimer <= 0)
            {
                Shoot();
                m_shootTimer = m_TowerSO.ShootInterval;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent<Enemy>(out var enemy))
                {
                    m_enemiesInRange.Add(enemy);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent<Enemy>(out var enemy))
                {
                    m_enemiesInRange.Remove(enemy);
                }
            }
        }

        private void Shoot()
        {
            if (m_enemiesInRange.Count > 0)
            {
                Projectile projectile = Factory.GetObject(m_TowerSO.projectType);
                if (projectile == null) return;

                Vector2 shootDirection = (m_enemiesInRange[0].transform.position - transform.position).normalized;

                projectile.transform.position = transform.position;
                projectile.Launch(shootDirection);
            }
        }

        private void HandleEnemyDie(Enemy enemy)
        {
            if (m_enemiesInRange.Contains(enemy))
                m_enemiesInRange.Remove(enemy);
        }
    }

}
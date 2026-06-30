using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class Tower_Combat : MonoBehaviour
    {
        private TowerSO m_data;
        private CircleCollider2D m_circleCollider;
        private List<Enemy> m_enemiesInRange = new();
        private float m_shootTimer = 0f;

        private void Awake()
        {
            m_circleCollider = GetComponent<CircleCollider2D>();
        }

        private void OnEnable()
        {
            GameEvent.OnEnemyDie += HandleEnemyDestroyed;
        }

        private void OnDisable()
        {
            GameEvent.OnEnemyDie -= HandleEnemyDestroyed;
        }


        public void Init(TowerSO data)
        {
            m_data = data;
            m_circleCollider.radius = data.range;
            m_enemiesInRange = new List<Enemy>();
        }

        private void Update()
        {
            m_shootTimer -= Time.deltaTime;
            if (m_shootTimer <= 0)
            {
                m_shootTimer = m_data.shootInterval;
                Shoot();
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
                Projectile projectile = ProjectileFactory.Instance.GetObject(m_data.projectType);
                if (projectile == null) return;

                Vector2 shootDirection = (m_enemiesInRange[0].transform.position - transform.position).normalized;

                projectile.transform.position = transform.position;
                projectile.Shoot(m_data, shootDirection);
            }
        }

        private void HandleEnemyDestroyed(Enemy enemy) => m_enemiesInRange.Remove(enemy);
    }

}
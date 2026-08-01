using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public class TowerCombat : MonoBehaviour
    {
        private TowerBase Tower;
        private List<Enemy> _enemiesInRange;
        private FactoryManager FactoryManager => GameManager.Instance.FactoryManager;

        [SerializeField] private SpriteRenderer _SpriteRenderer;

        private float _shootTimer = 0f;

        private void Awake()
        {
            _enemiesInRange = new List<Enemy>();

            Tower = GetComponentInParent<Tower>();

            if (Tower != null && TryGetComponent<CircleCollider2D>(out var collider))
            {
                collider.radius = Tower.Ability.ShootRange;
            }
            if (_SpriteRenderer != null) _SpriteRenderer.enabled = false;
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
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0)
            {
                Shoot();
                _shootTimer = Tower.Ability.ShootInterval;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Enemy>(out var enemy))
            {
                _enemiesInRange.Add(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && collision.TryGetComponent<Enemy>(out var enemy))
            {
                _enemiesInRange.Remove(enemy);
            }
        }

        private void Shoot()
        {
            if (_enemiesInRange.Count > 0)
            {
                Projectile projectile = FactoryManager.GetProjectile(Tower.TowerSO.projectType);
                if (projectile == null) return;

                Vector2 shootDirection = (_enemiesInRange[0].transform.position - transform.position).normalized;

                projectile.transform.position = transform.position;
                projectile.Launch(shootDirection);
            }
        }

        private void HandleEnemyDie(Enemy enemy)
        {
            if (_enemiesInRange.Contains(enemy))
                _enemiesInRange.Remove(enemy);
        }

        private void ShowShootRangeArea()
        {
            if (Tower.TowerSO == null || _SpriteRenderer == null) return;

            float dimeter = Tower.TowerSO.Ability.ShootRange * 2f;
            transform.localScale = Vector3.one * dimeter;
            _SpriteRenderer.enabled = true;
        }
    }

}
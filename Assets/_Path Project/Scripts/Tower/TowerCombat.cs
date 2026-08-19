using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TowerCombat : MonoBehaviour
    {
        private GameManager GameManager => GameManager.Instance;
        private TowerBase Tower;
        private List<Enemy> _enemiesInRange;
        // private FactoryManager FactoryManager => GameManager.Instance.FactoryManager;

        public SpriteRenderer ShootRanageArea;

        private float _shootTimer = 0f;

        private void Awake()
        {
            _enemiesInRange = new List<Enemy>();
            Tower = GetComponentInParent<Tower>();
            ShootRanageArea = GetComponent<SpriteRenderer>();
        }


        private void OnEnable()
        {
            EnemyEvent.EnemyDie += EnemyEvent_EnemyDie;
        }

        private void OnDisable()
        {
            EnemyEvent.EnemyDie -= EnemyEvent_EnemyDie;
        }

        private void Start()
        {
            if (Tower != null && TryGetComponent<CircleCollider2D>(out var collider))
            {
                collider.radius = Tower.Ability.ShootRange;
            }
            SetShootRangeArea();
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
                Projectile projectile = GameManager.FactoryManager.GetProjectile(Tower.SO.ProjectileSO);
                if (projectile == null) return;

                Vector2 shootDirection = (_enemiesInRange[0].transform.position - transform.position).normalized;

                projectile.transform.position = transform.position;
                projectile.Launch(shootDirection);
            }
        }

        private void EnemyEvent_EnemyDie(Enemy enemy)
        {
            if (_enemiesInRange.Contains(enemy))
                _enemiesInRange.Remove(enemy);
        }

        private void SetShootRangeArea()
        {
            if (Tower.SO == null) return;

            float dimeter = Tower.SO.Ability.ShootRange * 2f;
            transform.localScale = Vector3.one * dimeter;
        }
    }

}
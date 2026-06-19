using UnityEngine;
using System.Collections.Generic;
using System;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData _data;
    private CircleCollider2D _circleCollider;
    public List<TDEnemy> _enemiesInRange = new List<TDEnemy>();
    private Object_Pool _projectilePool;

    private float _shootTimer;

    [SerializeField] private bool _displayDrawGimoz;

    private void OnEnable()
    {
        TDEnemy.OnEnemyDestroyed += HandleEnemeyDestroyed;
    }

    private void OnDisable()
    {
        TDEnemy.OnEnemyDestroyed -= HandleEnemeyDestroyed;
    }

    void Start()
    {
        _projectilePool = GetComponent<Object_Pool>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = _data.range;

        _enemiesInRange = new List<TDEnemy>();
        _shootTimer = _data.shootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0)
        {
            _shootTimer = _data.shootInterval;
            Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            TDEnemy enemy = collision.GetComponent<TDEnemy>();
            if (enemy != null)
            {
                _enemiesInRange.Add(enemy);
                // Optionally, start shooting at the enemy here
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TDEnemy enemy = collision.GetComponent<TDEnemy>();
            if (enemy != null)
            {
                _enemiesInRange.Remove(enemy);
                // Optionally, stop shooting at the enemy here
            }
        }
    }

    private void Shoot()
    {
        if (_enemiesInRange.Count > 0)
        {
            GameObject projectile = _projectilePool.GetObject();
            projectile.transform.position = transform.position;
            projectile.SetActive(true);
            Vector2 shootDirection = (_enemiesInRange[0].transform.position - transform.position).normalized;
            projectile.GetComponent<Projectile>().Shoot(_data, shootDirection);
        }
    }

    private void HandleEnemeyDestroyed(TDEnemy enemy)
    {
        _enemiesInRange.Remove(enemy);
    }

    private void OnDrawGizmos()
    {
        if (!_displayDrawGimoz) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _data.range);
    }
}

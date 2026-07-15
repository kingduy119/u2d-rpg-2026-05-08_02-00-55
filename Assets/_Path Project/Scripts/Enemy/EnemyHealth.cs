
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace TDGame
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private Transform _healthBar;

        private Vector3 _healthBarOriginalScale;
        private HealthState _healthState;
        private Enemy _enemy;


        private void Awake()
        {
            _healthState = new();
            _healthBarOriginalScale = _healthBar.localScale;
        }

        private void Update()
        {
            if (_healthState.IsDirty)
            {
                UpdateHealthUI();
                _healthState.Clearn();
            }
        }

        public void Init(Enemy enemy)
        {
            _enemy = enemy;
            _healthState.Health = enemy.Data.health;
            _healthState.MaxHealth = enemy.Data.maxHealth;
        }

        public void TakeDamage(TowerSO data)
        {
            _healthState.Health -= data.damage;
            if (_healthState.Health <= 0)
            {
                GameEvent.SendEnemyDie(_enemy);
                _enemy.Deactive();
            }
        }

        private void UpdateHealthUI()
        {
            Vector3 scale = _healthBarOriginalScale;
            scale.x = _healthBarOriginalScale.x * _healthState.Percent;
            _healthBar.localScale = scale;
        }
    }

}
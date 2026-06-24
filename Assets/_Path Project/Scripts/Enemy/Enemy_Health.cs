
using System;
using UnityEngine;

namespace TDGame
{
    public class Enemy_Health : MonoBehaviour
    {
        public event Action OnEnemyDie;

        [SerializeField] private Transform _healthBar;


        private float _health;
        private float _maxHealth;
        private Vector3 _healthBarOriginalScale;


        private void Awake()
        {
            _healthBarOriginalScale = _healthBar.localScale;
        }

        private void Start()
        {
            UpdateHealthUI();
        }

        public void Initialize(EnemyData data)
        {
            _health = data.health;
            _maxHealth = data.maxHealth;
        }


        public void TakeDamge(TowerData data)
        {
            _health -= data.damage;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            if (_health <= 0)
            {
                OnEnemyDie?.Invoke();
                return;
            }

            UpdateHealthUI();
        }

        private void UpdateHealthUI()
        {
            float percent = _health / _maxHealth;
            Vector3 scale = _healthBarOriginalScale;
            scale.x = _healthBarOriginalScale.x * percent;
            _healthBar.localScale = scale;
        }
    }

}
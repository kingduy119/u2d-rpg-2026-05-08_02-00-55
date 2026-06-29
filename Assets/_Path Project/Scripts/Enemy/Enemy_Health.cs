
using System;
using Unity.VisualScripting;
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

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                UpdateHealthUI();
            }
        }

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
            UpdateHealthUI();
        }

        public void TakeDamage(TowerData data)
        {
            Health -= data.damage;
            Health = Mathf.Clamp(Health, 0, _maxHealth);
            if (Health <= 0)
            {
                OnEnemyDie?.Invoke();
                return;
            }
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
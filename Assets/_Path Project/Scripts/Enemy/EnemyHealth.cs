
using UnityEngine;

namespace TDGame
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform _healthBar;

        private Enemy _enemy;
        private Vector3 _healthBarOriginalScale;
        private HealthState _healthState = new();

        private void Awake()
        {
            _healthBarOriginalScale = _healthBar.localScale;

            if (TryGetComponent<Enemy>(out var enemy))
            {
                _enemy = enemy;
                _healthState.Health = enemy.Data.health;
                _healthState.MaxHealth = enemy.Data.maxHealth;
            }
        }

        private void Update()
        {
            if (_healthState.IsDirty)
            {
                UpdateHealthUI();
                _healthState.Clearn();
            }
        }

        public void TakeDamage(float amount)
        {
            _healthState.Health -= amount;
            if (_healthState.Health <= 0)
            {
                GameEvent.SendEnemyDie(_enemy);
                _enemy.Deactivate();
                _enemy.Sound.PlayDestroy();
                return;
            }
            _enemy.Sound.PlayTakeDamage();
        }

        private void UpdateHealthUI()
        {

            Vector3 scale = _healthBarOriginalScale;
            scale.x *= _healthState.Percent;
            _healthBar.localScale = scale;
        }
    }

}
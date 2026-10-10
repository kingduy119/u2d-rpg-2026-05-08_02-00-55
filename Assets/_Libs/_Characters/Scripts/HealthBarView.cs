

using UnityEngine;

namespace Characters
{
    public class HealthBarView : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform _healthBar;

        private float Health = 10f;
        private float MaxHealth = 10f;
        private float Percent => Health / MaxHealth;

        private void Awake()
        {
            if (_healthBar != null)
                UpdateUI();
        }

        public void Init(HealthData healthData)
        {
            Health = healthData.Health;
            MaxHealth = healthData.MaxHealth;
            UpdateUI();
        }

        public void TakeDamage(float amount)
        {
            if (Health > 0)
            {
                Health -= amount;
                UpdateUI();
                return;
            }

            Debug.Log("HealthBarView: Character is dead");
            if (TryGetComponent<Character>(out var character))
            {
                character.Dead();
            }
        }

        private void UpdateUI()
        {
            Vector3 scale = _healthBar.localScale;
            scale.x = Percent;
            _healthBar.localScale = scale;
        }
    }
}
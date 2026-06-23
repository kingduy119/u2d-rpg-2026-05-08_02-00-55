
using UnityEngine;

namespace TDGame
{
    public class Enemy_Health : MonoBehaviour
    {
        [SerializeField] private Transform _healthBar;

        private EnemyData m_data;
        private Vector3 _healthBarOriginalScale;

        public void Initialize(EnemyData data)
        {
            m_data = data;
        }

        private void Awake()
        {
            _healthBarOriginalScale = _healthBar.localScale;
        }

        private void OnEnable()
        {
            m_data.HealthChanged += OnHealthChange;

            UpdateUI();
        }

        private void OnDisable()
        {
            m_data.HealthChanged -= OnHealthChange;
        }

        public void TakeDamge(TowerData data)
        {
            m_data.TakeDamge(data.damage);
        }

        private void OnHealthChange()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            float percent = m_data.lives / m_data.maxLives;
            Vector3 scale = _healthBarOriginalScale;
            scale.x = _healthBarOriginalScale.x * percent;
            _healthBar.localScale = scale;
        }
    }

}
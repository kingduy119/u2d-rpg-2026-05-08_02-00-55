using UnityEngine;

namespace TDGame
{
    public class DirtyModel : ScriptableObject
    {
        public bool IsDirty { get; protected set; } = false;
        public void MarkDirty() => IsDirty = true;
        public void Clearn() => IsDirty = false;

        protected void SetValue<T>(ref T field, T value)
        {
            field = value;
            IsDirty = true;
        }

    }

    public class HealthModel : DirtyModel
    {
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;

        // private Vector3 _healthBarOriginalScale;

        public float Health
        {
            get => _health;
            set => SetValue(ref _health, value);
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set => SetValue(ref _maxHealth, value);
        }

        public float Percent => _health / _maxHealth;

        // private void OnValidate()
        // {
        //     UpdateUI();
        // }

        // public void UpdateUI()
        // {
        //     Vector3 scale = _healthBarOriginalScale;
        //     scale.x = _healthBarOriginalScale.x * Percent;
        //     _healthBar.localScale = scale;
        // }
    }
}
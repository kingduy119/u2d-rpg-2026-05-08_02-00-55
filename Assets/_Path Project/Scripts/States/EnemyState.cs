
namespace TDGame
{
    public class HealthState : DirtyState
    {
        protected float _health;
        protected float _maxHealth;

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
    }
}
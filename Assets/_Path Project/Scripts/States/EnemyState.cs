
namespace TDGame
{
    public class EnemyState : DirtyState
    {
        // public bool IsDirty { get; private set; } = false;
        private float _health;
        private float _maxHealth;

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                IsDirty = true;
            }
        }
    }
}
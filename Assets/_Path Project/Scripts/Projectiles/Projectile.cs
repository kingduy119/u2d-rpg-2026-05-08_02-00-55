using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class Projectile : MonoBehaviour,
        IPoolable<Projectile>
    {
        public virtual ProjectileType Type => ProjectileType.Default;
        public IObjectPool<Projectile> Pool { get; set; }
        private TowerSO _data;
        protected Vector3 _shotDirection;
        private float _projectileDuration;
        private float _speed = 1;

        public TowerSO Data => _data;

        protected virtual void Update()
        {
            HandleMovement();
        }

        public virtual void Launch(TowerSO data, Vector3 shotDirection)
        {
            _data = data;
            _shotDirection = shotDirection;
            _projectileDuration = data.projectileDuration;
            _speed = data.projectileSpeed;
        }

        protected virtual void HandleMovement()
        {
            if (_projectileDuration <= 0)
            {
                Deactivate();
                return;
            }

            _projectileDuration -= Time.deltaTime;
            transform.position += _speed * Time.deltaTime * _shotDirection;
        }

        public virtual void Deactivate() => Pool.Release(this);
    }

}
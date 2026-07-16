using UnityEngine;
using UnityEngine.Pool;
using EffectPack;

namespace TDGame
{
    public class Projectile : MonoBehaviour,
        IPoolable<Projectile>
    {
        public virtual ProjectileType Type { get; }
        public IObjectPool<Projectile> Pool { get; set; }
        // ###########

        private TowerSO _data;
        protected Vector3 _shotDirection;
        private float _projectileDuration;
        private float _speed = 1;

        protected virtual void Update()
        {
            HandleMovement();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            CheckCollisionInterfaces(collision);
            Deactivate();
        }

        private void CheckCollisionInterfaces(Collider2D collision)
        {
            var monoBehaviours = collision.gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in monoBehaviours)
            {
                HandleDamageableInterface(monoBehaviour);
                HandleEffectTriggerInterface(monoBehaviour);
            }
        }

        private void HandleDamageableInterface(MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour is IDamageable damageable)
            {
                damageable.TakeDamage(_data.damage);
            }
        }

        private void HandleEffectTriggerInterface(MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour is IEffectTrigger effectTrigger)
            {
                effectTrigger.TriggerEffect();
            }
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

        protected virtual void Deactivate() => Pool.Release(this);
    }

}
using UnityEngine;

namespace TDGame
{
    public class ProjectileCollision : MonoBehaviour
    {
        private Projectile m_Projectile;

        private void Awake()
        {
            if (TryGetComponent<Projectile>(out var instance))
            {
                m_Projectile = instance;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            CheckCollisionInterfaces(collision);
            m_Projectile.Deactivate();
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
                damageable.TakeDamage(m_Projectile.SO.Data.Damage);
            }
        }

        private void HandleEffectTriggerInterface(MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour is IEffectTrigger effectTrigger)
            {
                effectTrigger.TriggerEffect();
            }
        }
    }

}
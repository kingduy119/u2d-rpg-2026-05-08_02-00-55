using UnityEngine;

namespace TDGame
{
    public class ProjectileCollision : MonoBehaviour
    {
        public Projectile ProjectTile;

        void OnTriggerEnter2D(Collider2D collision)
        {
            CheckCollisionInterfaces(collision);
            ProjectTile.Deactivate();
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
                damageable.TakeDamage(ProjectTile.Data.damage);
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
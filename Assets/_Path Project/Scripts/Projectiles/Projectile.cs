using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public abstract class Projectile : MonoBehaviour
    {
        public abstract ProjectileType Type { get; }
        protected IObjectPool<Projectile> m_pool;
        public IObjectPool<Projectile> Pool
        {
            get => m_pool;
            set => m_pool = value;
        }
        // ###########

        private TowerSO m_data;
        private Vector3 m_shotDirection;
        private float m_projectileDuration;

        public virtual void Deactive()
        {
            m_pool.Release(this);
        }

        protected void Update()
        {
            HandleMovement();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy_Health enemy = collision.GetComponent<Enemy_Health>();
                enemy.TakeDamage(m_data);

                Deactivate();
            }
        }

        protected virtual void HandleMovement()
        {
            if (m_projectileDuration <= 0)
            {
                Deactivate();
            }
            else
            {
                m_projectileDuration -= Time.deltaTime;
                transform.position += m_data.projectileSpeed * Time.deltaTime * new Vector3(m_shotDirection.x, m_shotDirection.y);
            }
        }

        protected virtual void Deactivate() => m_pool.Release(this);

        public void Shoot(TowerSO data, Vector3 shotDirection)
        {
            m_data = data;
            m_shotDirection = shotDirection;
            m_projectileDuration = data.projectileDuration;
        }
    }

}
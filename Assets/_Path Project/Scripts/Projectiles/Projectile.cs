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
    // public class Projectile : MonoBehaviour
    // {
    //     private TowerSO _data;
    //     private Vector3 _shotDirection;
    //     private float _projectileDuration;

    //     private IObjectPool<Projectile> _objectPool;
    //     public IObjectPool<Projectile> ObjectPool { set => _objectPool = value; }


    //     // Update is called once per frame
    //     void Update()
    //     {
    //         if (_projectileDuration <= 0)
    //         {
    //             gameObject.SetActive(false);
    //         }
    //         else
    //         {
    //             _projectileDuration -= Time.deltaTime;
    //             transform.position += _data.projectileSpeed * Time.deltaTime * new Vector3(_shotDirection.x, _shotDirection.y);
    //         }
    //     }


    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Enemy"))
    //     {
    //         Enemy_Health enemy = collision.GetComponent<Enemy_Health>();
    //         enemy.TakeDamge(_data);

    //         Deactivate();
    //     }
    // }

    //     public void Shoot(TowerSO data, Vector3 shotDirection)
    //     {
    //         _data = data;
    //         _shotDirection = shotDirection;
    //         _projectileDuration = data.projectileDuration;
    //     }

    //     public void Deactivate()
    //     {
    //         _objectPool.Release(this);
    //     }
    // }
}
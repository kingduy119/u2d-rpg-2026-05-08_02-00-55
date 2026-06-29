using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        public abstract ProjectileType Type { get; }
        protected IObjectPool<ProjectileBase> m_pool;
        public IObjectPool<ProjectileBase> Pool
        {
            get => m_pool;
            set => m_pool = value;
        }
        // ###########

        private TowerData m_data;
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

        protected virtual void HandleMovement()
        {
            if (m_projectileDuration <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                m_projectileDuration -= Time.deltaTime;
                transform.position += m_data.projectileSpeed * Time.deltaTime * new Vector3(m_shotDirection.x, m_shotDirection.y);
            }
        }

        public void Shoot(TowerData data, Vector3 shotDirection)
        {
            m_data = data;
            m_shotDirection = shotDirection;
            m_projectileDuration = data.projectileDuration;
        }
    }

    public class ProjectileDefault : ProjectileBase
    {
        public override ProjectileType Type => ProjectileType.Default;
    }

    public class Arrow : ProjectileBase
    {
        public override ProjectileType Type => ProjectileType.Arrow;
    }

}
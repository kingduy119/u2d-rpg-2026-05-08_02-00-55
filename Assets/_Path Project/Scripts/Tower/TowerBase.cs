using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public abstract class TowerBase : MonoBehaviour,
        IPoolable<TowerBase>
    {
        [SerializeField] private SpriteRenderer m_render;
        [SerializeField] private bool m_showDraw;


        public TowerSO TowerSO;
        public Vector2Int Size => TowerSO.size;
        public IObjectPool<TowerBase> Pool { get; set; }

        private void OnValidate()
        {
            if (!TowerSO) return;
            m_render.sprite = TowerSO.sprite;
        }
        protected void Awake()
        {
            if (TowerSO != null)
            {
                m_render.sprite = TowerSO.sprite;
            }

            // if (_ProjectilePrefab != null)
            //     ProjectilePool = new(_ProjectilePrefab, transform);
        }

        public virtual void Deactivate() => Pool.Release(this);
        private void OnDrawGizmos()
        {
            if (m_showDraw)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, TowerSO.ShootRange);
            }
        }
    }
}

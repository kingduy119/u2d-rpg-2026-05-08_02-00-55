using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public interface IHoverable
    {
        void SetHover(bool value);
    }
    public abstract class TowerBase : MonoBehaviour,
        IPoolable<TowerBase>,
        IHoverable
    {
        [SerializeField] private bool m_showDraw;
        [SerializeField] private SpriteRenderer m_render;

        public TowerSO TowerSO;
        public IObjectPool<TowerBase> Pool { get; set; }

        private void OnValidate()
        {
            if (!TowerSO) return;
            m_render.sprite = TowerSO.sprite;
        }
        protected virtual void Awake()
        {
            if (TowerSO != null)
            {
                m_render.sprite = TowerSO.sprite;
            }
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
        public void Log()
        {
            Debug.Log($"Tower: {gameObject.name}");
        }

        public abstract void SetHover(bool value);

    }
}

using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public interface ITower
    {

    }

    [RequireComponent(typeof(Tower_Combat))]

    public abstract class TowerBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_render;
        [SerializeField] private TowerSO m_data;
        [SerializeField] private bool m_showDraw;

        // ###########
        public TowerType Type = TowerType.Tower;
        public IObjectPool<TowerBase> Pool { get; set; }
        // ###########

        private Tower_Combat m_combat;

        public Vector2Int Size => m_data.size;

        private void OnValidate()
        {
            if (!m_data) return;
            m_render.sprite = m_data.sprite;
        }
        protected void Awake()
        {
            m_combat = GetComponent<Tower_Combat>();

            if (m_data != null)
            {
                Type = m_data.towerType;
                m_render.sprite = m_data.sprite;
            }
        }

        private void Start()
        {
            m_combat.Init(m_data);
        }

        public virtual void Deactivate() => Pool.Release(this);

        private void OnDrawGizmos()
        {
            if (m_showDraw)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, m_data.range);
            }
        }
    }
}

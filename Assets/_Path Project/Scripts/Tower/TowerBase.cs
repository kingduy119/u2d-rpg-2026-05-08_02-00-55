using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    [RequireComponent(typeof(Tower_Combat))]

    public abstract class TowerBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_render;
        [SerializeField] private TowerSO m_data;
        [SerializeField] private bool m_showDraw;

        private Tower_Combat m_combat;
        protected IObjectPool<TowerBase> m_pool;

        private void OnValidate()
        {
            if (!m_data) return;
            m_render.sprite = m_data.sprite;
        }
        protected void Awake()
        {
            m_combat = GetComponent<Tower_Combat>();
        }

        private void Start()
        {
            m_combat.Init(m_data);
        }

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

using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public abstract class TowerBase : MonoBehaviour,
        IPoolable<TowerBase>
    {
        [SerializeField] private bool m_showDraw;
        [SerializeField] private SpriteRenderer m_render;

        public TowerSO TowerSO;
        public TowerAbility Ability;
        public IObjectPool<TowerBase> Pool { get; set; }

        protected void OnValidate()
        {
            if (TowerSO == null) return;

            m_render.sprite = TowerSO.sprite;
        }

        protected virtual void Awake()
        {
            if (TowerSO == null) return;

            m_render.sprite = TowerSO.sprite;
            Ability = new(TowerSO.Ability);
        }

        public virtual void Deactivate() => Pool.Release(this);

        // private void OnDrawGizmos()
        // {
        //     if (m_showDraw)
        //     {
        //         Gizmos.color = Color.red;
        //         Gizmos.DrawWireSphere(transform.position, TowerSO.Ability.ShootRange);
        //     }
        // }
    }
}

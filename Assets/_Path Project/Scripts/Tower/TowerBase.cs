using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public abstract class TowerBase : MonoBehaviour
    {
        [SerializeField] private bool _ShowDraw;
        [SerializeField] private SpriteRenderer m_render;

        public TowerSO TowerSO;
        public TowerAbility Ability;

        public TowerCombat Combat;
        public TowerHover Hover;

        protected virtual void OnValidate()
        {
            if (TowerSO == null) return;

            m_render.sprite = TowerSO.sprite;
        }

        protected virtual void Awake()
        {
            if (TryGetComponent<TowerHover>(out var hover))
            {
                Hover = hover;
            }
            Combat = gameObject.GetComponentInChildren<TowerCombat>();


            if (TowerSO == null) return;

            m_render.sprite = TowerSO.sprite;
            Ability = new(TowerSO.Ability);
        }

        protected virtual void Start() { }

        private void OnDrawGizmos()
        {
            if (_ShowDraw)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, TowerSO.Ability.ShootRange);
            }
        }

        public abstract void Deactivate();
    }
}

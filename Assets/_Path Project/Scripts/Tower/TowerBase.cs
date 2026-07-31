using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public abstract class TowerBase : MonoBehaviour,
        IPoolable<TowerBase>
    {
        [SerializeField] private bool _ShowDraw;
        [SerializeField] private SpriteRenderer m_render;

        public TowerSO TowerSO;
        public TowerAbility Ability;
        public IObjectPool<TowerBase> Pool { get; set; }

        protected TowerCombat _Combat;
        protected TowerHover _Hover;

        protected virtual void OnValidate()
        {
            if (TowerSO == null) return;

            m_render.sprite = TowerSO.sprite;
        }

        protected virtual void Awake()
        {
            if (TryGetComponent<TowerHover>(out var hover))
            {
                _Hover = hover;
            }
            _Combat = gameObject.GetComponentInChildren<TowerCombat>();


            if (TowerSO == null) return;

            m_render.sprite = TowerSO.sprite;
            Ability = new(TowerSO.Ability);
        }

        protected virtual void Start()
        {
            Init();
        }

        public virtual void TowerUP() { }
        public virtual void Deactivate() => Pool.Release(this);

        public virtual void MarkBuilded()
        {
            if (_Hover != null) _Hover.enabled = true;
            if (_Combat != null) _Combat.enabled = true;
        }

        protected virtual void Init()
        {
            if (_Hover != null) _Hover.enabled = false;
            if (_Combat != null) _Combat.enabled = false;
        }


        private void OnDrawGizmos()
        {
            if (_ShowDraw)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, TowerSO.Ability.ShootRange);
            }
        }
    }
}

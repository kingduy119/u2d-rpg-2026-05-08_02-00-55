

using UnityEngine;

namespace TDGame
{
    public class Tower : TowerBase
    {
        private static readonly int IsHoverHash = Animator.StringToHash("IsHover");
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            if (TryGetComponent<Animator>(out var animator))
            {
                _animator = animator;
            }
        }

        public override void SetHover(bool value)
        {
            if (_animator != null) _animator.SetBool(IsHoverHash, value);
        }
    }
}
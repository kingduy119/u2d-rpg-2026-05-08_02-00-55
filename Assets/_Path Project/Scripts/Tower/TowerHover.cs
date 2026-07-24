

using UnityEngine;

namespace TDGame
{

    public class TowerHover : MonoBehaviour,
        IHoverable
    {
        private static readonly int IsHoverHash = Animator.StringToHash("IsHover");
        private Animator _animator;

        private void Awake()
        {
            if (TryGetComponent<Animator>(out var animator))
            {
                _animator = animator;
            }
        }

        public void SetHover(bool value)
        {
            if (_animator != null) _animator.SetBool(IsHoverHash, value);
        }
    }
}
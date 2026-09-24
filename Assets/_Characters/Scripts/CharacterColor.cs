using UnityEngine;

namespace Characters
{


    public class CharacterColor : MonoBehaviour
    {
        [SerializeField] protected Colors _color = Colors.Blue;
        [SerializeField] private SpriteRenderer _spriteRender;
        [SerializeField] private CharacterSO _Data;
        private Animator _animator;

        private void OnValidate()
        {
            SetSprite(_color);
        }

        void Start()
        {
            if (TryGetComponent<Animator>(out var animator))
            {
                _animator = animator;
                SetColors(_color);
            }
        }

        public void SetColors(Colors color)
        {
            _animator.runtimeAnimatorController = color switch
            {
                Colors.Blue => _Data.CharacterColors.BlueAnimController,
                Colors.Red => _Data.CharacterColors.RedAnimController,
                _ => _Data.CharacterColors.BlueAnimController
            };
        }

        private void SetSprite(Colors color)
        {
            _spriteRender.sprite = color switch
            {
                Colors.Red => _Data.CharacterColors.RedSprite,
                _ => _Data.CharacterColors.BlueSprite
            };
        }
    }
}
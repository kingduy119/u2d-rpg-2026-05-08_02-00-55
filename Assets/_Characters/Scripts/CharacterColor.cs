using System.Collections.Generic;
using UnityEngine;

namespace Characters
{


    public class CharacterColor : MonoBehaviour
    {
        [SerializeField] protected CterType _cterType = CterType.Warrior;
        [SerializeField] protected Colors _color = Colors.Blue;

        [SerializeField] private SpriteRenderer _spriteRender;
        [SerializeField] private CharacterSO _Data;
        [SerializeField] private Animator _animator;

        [SerializeField] private List<CharacterSO> _character;

        private void OnValidate()
        {
            SetCharater(_cterType);
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

        public void SetCharater(CterType type)
        {
            var entry = _character.Find(cter => cter.Type == type);

            if (entry != null)
                _Data = entry;
        }

        public void SetColors(Colors color)
        {
            var cterColor = _Data.colors.Find(entry => entry.color == color);

            if (cterColor != null)
                _animator.runtimeAnimatorController = cterColor.AnimController;
            // _animator.runtimeAnimatorController = color switch
            // {
            //     Colors.Blue => _Data.CharacterColors.BlueAnimController,
            //     Colors.Red => _Data.CharacterColors.RedAnimController,
            //     _ => _Data.CharacterColors.BlueAnimController
            // };
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
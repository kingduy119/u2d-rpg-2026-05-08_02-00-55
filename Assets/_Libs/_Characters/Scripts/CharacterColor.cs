using UnityEngine;

namespace Characters
{

    public class CharacterColor : MonoBehaviour
    {
        [SerializeField] protected Colors _color = Colors.Blue;

        [Header("Art Settings")]
        [SerializeField] private SpriteRenderer _spriteRender;
        [SerializeField] private Animator _animator;
        [SerializeField] private Character _character;


        private void OnValidate()
        {
            UpdateColor(_character.DefaultSO);
        }

        public void UpdateColor(CharacterSO cterSO)
        {
            if (!cterSO) return;

            var characterSetting = cterSO.colors.Find(entry => entry.color == _color);
            if (characterSetting != null)
            {
                _animator.runtimeAnimatorController = characterSetting.AnimController;
                _spriteRender.sprite = characterSetting.Sprite;
            }

        }
    }
}
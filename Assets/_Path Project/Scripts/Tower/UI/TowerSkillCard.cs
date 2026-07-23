
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public static class TowerEvent
    {
        public static event Action<SkillCardSO> OnSkillCardSelect;
        public static void RaiseSkillCardSelect(SkillCardSO so) => OnSkillCardSelect?.Invoke(so);

        public static void RaiseAbilitySelect(Ability ability)
        {
            ability.Use();
        }
    }
    public class TowerSkillCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _thumbnail;

        public SkillCardSO SO;
        public Ability _ability;

        private void OnValidate()
        {
            if (_ability != null)
            {
                _thumbnail.sprite = _ability._image;
                _titleText.text = _ability._name;
                _descriptionText.text = _ability._description;
            }
        }

        public void OnCardSelect() => TowerEvent.RaiseSkillCardSelect(SO);

        public void OnTestAbility() => TowerEvent.RaiseAbilitySelect(_ability);
    }
}
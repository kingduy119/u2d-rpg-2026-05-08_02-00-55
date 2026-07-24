
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public class TowerSkillCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _thumbnail;

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

        public void OnAbilitySelect() => TowerEvent.RaiseAbilitySelect(_ability);
    }
}
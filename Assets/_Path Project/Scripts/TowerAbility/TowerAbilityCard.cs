
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public class TowerAbilityCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;

        [SerializeField] private Image _thumbnail;

        private Ability _ability;

        private void OnValidate()
        {
            Initialize();
        }

        public void Initialize(Ability ability = null)
        {
            _ability = ability;
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
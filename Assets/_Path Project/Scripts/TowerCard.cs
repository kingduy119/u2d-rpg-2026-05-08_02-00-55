using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public class TowerCard : MonoBehaviour
    {
        [SerializeField] private Image towerImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text costText;

        [SerializeField] private TowerSO _data;
        public static event Action<TowerSO> OnTowerCardSelected;

        private void OnValidate()
        {
            if (_data != null)
            {
                towerImage.sprite = _data.sprite;
                nameText.text = _data.towerName;
                costText.text = _data.cost.ToString();
            }
        }

        public void Initialize(TowerSO data)
        {
            _data = data;
            towerImage.sprite = data.sprite;
            nameText.text = data.towerName;
            costText.text = data.cost.ToString();
        }

        public void PlaceTowerClick()
        {
            OnTowerCardSelected?.Invoke(_data);
        }
    }

}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public class TowerSelectSlot : ButtonBase
    {
        [SerializeField] private Image m_image;
        [SerializeField] private TMP_Text m_priceText;

        public TowerSO TowerSO;

        private void OnValidate()
        {
            UpdateUI();
        }

        private void Start()
        {
            UpdateUI();
        }


        protected override void HandleClick()
        {
            TowerEvent.TowerBuildSlotClick?.Invoke(TowerSO);
        }

        public void Initialize(TowerSO data)
        {
            TowerSO = data;
            UpdateUI();
        }

        public void CheckActivve(GamePlayState state)
        {
            bool enable = state.Golds >= TowerSO.Price.Gold;
            _Button.interactable = enable;
        }

        private void UpdateUI()
        {
            if (TowerSO)
            {
                m_image.sprite = TowerSO.sprite;
                m_priceText.text = $"{TowerSO.Price.Gold}";
            }
        }
    }
}
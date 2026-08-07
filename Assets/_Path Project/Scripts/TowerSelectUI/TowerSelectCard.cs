using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public class TowerSelectCard : ButtonBase//MonoBehaviour
    {
        [SerializeField] private Image m_image;
        [SerializeField] private TMP_Text m_priceText;

        public TowerSO TowerSO;

        private void OnValidate()
        {
            UpdateUI();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateUI();
        }

        private void Update()
        {
            if (TowerSO != null)
            {
                _Button.interactable = GameManager.Instance.GameStates.GamePlayState.Golds >= TowerSO.cost;
            }
        }

        protected override void HandleClick()
        {
            TowerEvent.OnTowerCardSelect?.Invoke(TowerSO);
        }

        public void Initialize(TowerSO data)
        {
            TowerSO = data;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (TowerSO)
            {
                m_image.sprite = TowerSO.sprite;
                m_priceText.text = $"{TowerSO.cost}";
            }
        }
    }
}
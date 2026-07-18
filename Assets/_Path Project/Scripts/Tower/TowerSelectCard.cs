using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public class TowerSelectCard : MonoBehaviour
    {
        [SerializeField] private Image m_image;
        [SerializeField] private TMP_Text m_priceText;
        [SerializeField] private Button m_button;

        private TowerSO m_data;
        private InGameState InGameState;

        private void Awake()
        {
            InGameState = GameManager.Instance.InGameState;
            m_button.onClick.AddListener(OnCardClick);
        }

        private void OnEnable()
        {
            UpdateUI();
        }

        private void Update()
        {
            if (m_data != null)
            {
                m_button.interactable = InGameState.Golds >= m_data.cost;
            }
        }

        public void Initialize(TowerSO data)
        {
            m_data = data;
            UpdateUI();
        }

        public void OnCardClick()
        {
            GameEvent.HandleTowerSelect(m_data);
        }

        private void UpdateUI()
        {
            if (m_data)
            {
                m_image.sprite = m_data.sprite;
                m_priceText.text = $"{m_data.cost}";
            }
        }
    }
}
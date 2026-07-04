using TMPro;
using Unity.VisualScripting;
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

        private void Awake()
        {
            m_button.onClick.AddListener(OnCardClick);
        }

        private void OnEnable()
        {
            UpdateUI();
        }

        public void Initialize(TowerSO data)
        {
            m_data = data;
            UpdateUI();
        }

        public void OnCardClick()
        {
            GameEvent.TowerSelect(m_data);
        }

        private void Update()
        {
            if (m_data != null)
            {
                m_button.interactable = GameManager.Instance.InGame.Golds >= m_data.cost;
            }
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
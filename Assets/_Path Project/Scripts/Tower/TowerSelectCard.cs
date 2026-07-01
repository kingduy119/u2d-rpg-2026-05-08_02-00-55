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

        private TowerSO m_data;



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
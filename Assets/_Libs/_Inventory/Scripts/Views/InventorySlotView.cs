

using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryLib
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] GameObject _stackTextContainer;
        [SerializeField] TMP_Text _stackText;

        [Header("Item Settings")]
        public ItemDto m_ItemDto;
        public InventoryData m_InventoryData;

        void OnValidate()
        {
            UpdateView(m_ItemDto);
        }

        void Awake()
        {
            Clear();
        }

        public bool Empty => m_ItemDto == null;

        public void SetData(InventoryData data, ItemDto dto)
        {
            m_InventoryData = data;
            UpdateView(dto);
        }

        void UpdateView(ItemDto dto)
        {
            bool hasDto = dto != null;
            if (_icon) _icon.gameObject.SetActive(hasDto);
            if (_stackTextContainer) _stackTextContainer.gameObject.SetActive(hasDto);
            if (!hasDto) return;

            m_ItemDto = dto;
            if (_icon) _icon.sprite = dto.Icon;
            if (_stackText) _stackText.text = $"{m_InventoryData.Stack}";
        }

        public void Clear()
        {
            m_ItemDto = null;
            m_InventoryData = null;
            UpdateView(null);
        }
    }

}
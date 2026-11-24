

using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;

namespace InventoryLib
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] TMP_Text _StackText;
        [SerializeField] InventorySlotView[] _slots;
        public ItemDto[] Dtos;

        readonly Dictionary<string, ItemDto> MapDtos = new();
        private InventoryData[] _datas;

        public void SetData(InventoryData[] datas)
        {
            _datas = datas;
        }

        void Awake()
        {
            foreach (var dto in Dtos)
            {
                if (dto != null) MapDtos.Add(dto.Id, dto);
            }
            ClearSlots();
        }

        void OnEnable()
        {
            if (_datas != null) FillSlots(_datas);
            if (_StackText) _StackText.text = $"{_datas?.Length ?? 0}/{_slots.Length}";


            ButtonEvents.InventorySlotClick += OnInventorySlotClick;
        }
        void OnDisable()
        {
            ClearSlots();

            ButtonEvents.InventorySlotClick += OnInventorySlotClick;
        }


        ButtonView prevItem;
        private void OnInventorySlotClick(ButtonView buttonView)
        {
            if (prevItem != null) prevItem.UnSelected();

            buttonView.Selected();
            prevItem = buttonView;
        }

        private void ClearSlots()
        {
            foreach (var slot in _slots)
            {
                slot.Clear();
            }
        }

        public void FillSlots(InventoryData[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                AddSlot(datas[i]);
            }
        }

        public void AddSlot(InventoryData data)
        {
            if (data == null) return;

            var slot = GetEmptySlot();
            var dto = MapDtos.ContainsKey(data.Id) ? MapDtos[data.Id] : null;

            if (slot) slot.SetData(data, dto);
        }

        private InventorySlotView GetEmptySlot()
        {
            foreach (var slot in _slots)
            {
                if (slot.Empty) return slot;
            }
            return null;
        }
    }

}


using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] InventorySlotView[] _slots;
        public ItemDto[] Dtos;
        public InventoryData[] Datas;
        public List<InventoryData> DatasList => new(Datas);

        readonly Dictionary<string, ItemDto> MapDtos = new();

        void Start()
        {
            foreach (var dto in Dtos)
            {
                MapDtos.Add(dto.Id, dto);
            }

            // for (int i = 0; i < Datas.Length; i++)
            // {
            //     var slot = _slots[i];
            //     var dto = MapDtos.ContainsKey(Datas[i].Id) ? MapDtos[Datas[i].Id] : null;

            //     if (slot) slot.SetData(Datas[i], dto);
            // }
        }

        public void FillSlots(InventoryData[] datas)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                var slot = GetEmptySlot();
                var dto = MapDtos.ContainsKey(datas[i].Id) ? MapDtos[datas[i].Id] : null;

                if (slot) slot.SetData(datas[i], dto);
            }
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
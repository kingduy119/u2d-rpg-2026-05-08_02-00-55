

using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryLib
{
    [Serializable]
    public class InventoryData
    {
        public string Id;
        public int Stack = 1;
        public int MaxStack = 1;
    }

    [Serializable]
    public class ListInventoryData
    {
        public InventoryData[] Datas;
    }

    // [CreateAssetMenu(fileName = "ItemDto", menuName = "Inventory/ItemDto")]
    // public class ItemDto : ScriptableObject
    // {
    //     public string Id;
    //     public ItemType Type;
    //     public Sprite Icon;

    //     [Header("Stack")]
    //     public int MaxStack;
    // }

}

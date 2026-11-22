

using System;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "ItemDto", menuName = "Items/ItemDto")]
    public class ItemDto : ScriptableObject
    {
        public string Id;
        public ItemType Type;
        public Sprite Icon;

        [Header("Stack")]
        public int MaxStack;
    }


    [Serializable]
    public struct RewardItem
    {
        public ItemDto Item;
        public int Value;
    }
}
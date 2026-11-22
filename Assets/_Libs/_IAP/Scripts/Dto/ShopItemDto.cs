


using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace IAPLib
{
    // [CreateAssetMenu(fileName = "ShopItemDto", menuName = "Items/ShopItemDto")]
    public abstract class ShopItemDto : ScriptableObject
    {
        public ItemDto ItemIcon;
        public string ItemName;
        public string ItemDesc;

        public ResourceData PurchaseResource;

        [Header("Reward")]
        // public List<RewardData> Rewards;
        public List<RewardItem> Rewards;
    }
}

using Core;
using UnityEngine;

namespace IAPLib
{
    [CreateAssetMenu(fileName = "ShopIapResourceDto", menuName = "Items/ShopIapResourceDto")]
    public class ShopIapResourceDto : ShopItemDto
    {
        [Header("Store Setting")]
        public ItemDto PurchaseDTO;
        public string PurchaseText;
    }
}



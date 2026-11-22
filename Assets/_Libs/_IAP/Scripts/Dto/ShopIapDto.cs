
using Core;
using UnityEngine;


namespace IAPLib
{
    [CreateAssetMenu(fileName = "ShopIapDto", menuName = "Items/ShopIapDto")]
    public class ShopIapDto : ShopItemDto
    {
        [Header("Store Setting")]
        public IAPProductKeys ProductId;
        public ItemDto PurchaseDTO;
        public string PurchaseText;

    }
}
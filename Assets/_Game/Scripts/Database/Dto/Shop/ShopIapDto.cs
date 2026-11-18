
using UnityEngine;

[CreateAssetMenu(fileName = "ShopIapDto", menuName = "Shop/ShopIapDto")]
public class ShopIapDto : ShopItemDto
{
    [Header("Store Setting")]
    public IAPProductKeys ProductId;
    public ItemDTO PurchaseDTO;
    public string PurchaseText;

}
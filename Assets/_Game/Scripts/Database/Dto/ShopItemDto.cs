
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemDto", menuName = "Shop/ShopItemDto")]
public class ShopItemDto : BaseItemSO
{
    public string GetPriceString() => $"{PurchasePrice}{Currency}";
}
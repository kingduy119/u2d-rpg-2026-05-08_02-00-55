
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "ShopAdsItemDto", menuName = "Shop/ShopAdsItemDto")]
public class ShopAdsItemDto : ShopItemDto
{
    public virtual ItemSlotType Type => ItemSlotType.Ads;
}
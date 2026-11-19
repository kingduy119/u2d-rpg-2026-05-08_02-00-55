
using UnityEngine;
using System.Collections.Generic;

public abstract class ShopItemDto : ScriptableObject
{
    public virtual ItemSlotType Type => ItemSlotType.Iap;
    public ItemDTO ItemIcon;
    public string ItemName;
    public string ItemDesc;

    public ResourceData PurchaseResource;

    [Header("Reward")]
    public List<RewardData> Rewards;
}

[CreateAssetMenu(fileName = "PurchaseResourceDto", menuName = "Shop/PurchaseResourceDto")]
public class PurchaseResourceDto : ShopItemDto
{
    [Header("Store Setting")]
    public ItemDTO PurchaseDTO;
    public string PurchaseText;
}
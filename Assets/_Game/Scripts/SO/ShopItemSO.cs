
// public class ItemSlotSO :
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gold,
    Diamond,
    Currency
}
public enum ItemSlotType
{
    Ads,
    Iap,
    Default
}

public interface IShopItem
{
    void Init(ShopItemSO so);
}

public abstract class BaseItemSO : ScriptableObject
{
    public ItemSlotType Type = ItemSlotType.Default;
    public ItemType PurchasType;

    public Sprite ItemIcon;
    public string ItemName;
    public string ItemDesc;

    [Header("Store Setting")]
    public float PurchasePrice;
    public string Currency;

    [Header("Reward")]
    public int RewardValue;
    // public List<ItemSO> Rewards;
    public List<Reward> Rewards;
}


[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Shop/ShopItemSO")]
public class ShopItemSO : BaseItemSO
{
    public string GetPriceString() => $"{PurchasePrice}{Currency}";
}

[Serializable]
public class Reward
{
    public ItemSO Item;
    public int Value;
}
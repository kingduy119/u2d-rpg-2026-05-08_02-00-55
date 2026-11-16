
// public class ItemSlotSO :
using System;
using System.Collections.Generic;
using UnityEngine;



public interface IShopItem
{
    void Init(ShopItemSO so);
}

[Serializable]
public class RewardData
{
    public ItemDTO Item;
    public int Value;
}

[Serializable]
public class ResourcesData
{
    public ItemDTO Item;
    public float Value;
}

public abstract class BaseItemSO : ScriptableObject
{
    public ItemSlotType Type = ItemSlotType.Default;
    public ItemType PurchasType;

    public Sprite ItemIcon;
    public string ItemName;
    public string ItemDesc;

    [Header("Store Setting")]
    public ItemDTO PurchaseDTO;
    public float PurchasePrice;
    public string Currency;


    public List<ResourcesData> RequiredResources;

    [Header("Reward")]
    public int RewardValue;
    public List<RewardData> Rewards;
}


[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Shop/ShopItemSO")]
public class ShopItemSO : BaseItemSO
{
    public string GetPriceString() => $"{PurchasePrice}{Currency}";
}


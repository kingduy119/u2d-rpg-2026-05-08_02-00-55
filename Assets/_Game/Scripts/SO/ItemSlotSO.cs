
// public class ItemSlotSO :
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gold,
    Diamond,
    Other
}
public enum ItemSlotType
{
    Ads,
    Iap,
    Default
}

public abstract class BaseItemSO : ScriptableObject
{
    public Sprite ItemIcon;
    public string ItemName;
    public string ItemDesc;
}

[CreateAssetMenu(fileName = "ItemSlotSO", menuName = "Game/ItemSlotSO")]
public class ItemSlotSO : BaseItemSO
{
    public ItemSlotType Type = ItemSlotType.Default;
    public int StorePrice;
    public int Value;

    public List<ItemSO> AdsRewards;
}

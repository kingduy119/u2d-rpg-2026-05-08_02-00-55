using System;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;


[System.Serializable]
public class ShopItem
{
    public ItemSO itemSO;
    public int price;
}

public class ShopManager : MonoBehaviour
{
    // public static event Action<ShopManager, bool> OnShopStateChanged;

    // [SerializeField] private List<ShopItem> shopItems;
    [SerializeField] private ShopSlot[] shopSlots;

    [SerializeField] private InventoryManager inventoryManager;

    // public GameObject panel;
    // private void Start()
    // {
    //     // UpdateShopItems();
    //     // OnShopStateChanged?.Invoke(this, true);
    // }

    public void PopulateShop(List<ShopItem> shopItems)
    {
        for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
        {
            ShopItem shopItem = shopItems[i];
            shopSlots[i].Initalize(shopItem.itemSO, shopItem.price);
            shopSlots[i].gameObject.SetActive(true);
        }

        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }

    // public void UpdateShopItems()
    // {
    //     for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
    //     {
    //         ShopItem shopItem = shopItems[i];
    //         shopSlots[i].Initalize(shopItem.itemSO, shopItem.price);
    //         shopSlots[i].gameObject.SetActive(true);
    //     }

    //     for (int i = shopItems.Count; i < shopSlots.Length; i++)
    //     {
    //         shopSlots[i].gameObject.SetActive(false);
    //     }
    // }

    public bool HasSpaceForItem(ItemSO itemSO)
    {
        foreach (var slot in inventoryManager.itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity < itemSO.stackSize || slot.itemSO == null)
                return true;
        }
        return false;
    }

    public void BuyItem(ItemSO itemSO, int price)
    {
        if (itemSO != null && inventoryManager.gold >= price)
        {
            if (HasSpaceForItem(itemSO))
            {
                inventoryManager.gold -= price;
                inventoryManager.AddItem(itemSO, 1);
            }
        }
    }

    public void SellItem(ItemSO itemSO)
    {
        if (itemSO != null)
            return;

        foreach (var slot in shopSlots)
        {
            if (slot.itemSO == itemSO)
            {
                inventoryManager.gold += slot.price;
                return;
            }
        }
    }

    // private void Update()
    // {
    //     if (Keyboard.current.vKey.wasPressedThisFrame)
    //     {
    //         panel.gameObject.SetActive(!panel.gameObject.activeSelf);
    //     }
    // }
}

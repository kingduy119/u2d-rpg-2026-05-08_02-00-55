using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop_NPC : MonoBehaviour
{
    public Animator anim;
    public CanvasGroup shopCanvasGroup;
    private bool playerInRange = false;
    private bool isShopOpen = false;

    [Header("Shop Settings")]
    public ShopManager shopManager;
    [SerializeField] private List<ShopItem> itemsShop;
    [SerializeField] private List<ShopItem> weaponsShop;
    [SerializeField] private List<ShopItem> armorShop;
    public static event Action<ShopManager, bool> OnShopStateChanged;

    void Start()
    {
        HideShopPanel();
        shopManager.PopulateShop(itemsShop);
    }

    void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame && playerInRange)
        {
            if (!isShopOpen)
                OpenShopPanel();
            else
                HideShopPanel();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            anim.Play("monk_heal");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void OpenItemShop()
    {
        shopManager.PopulateShop(itemsShop);
    }

    public void OpenWeaponShop()
    {
        shopManager.PopulateShop(weaponsShop);
    }

    public void OpenArmorShop()
    {
        shopManager.PopulateShop(armorShop);
    }

    public void OpenShopPanel()
    {
        Time.timeScale = 0f;
        shopCanvasGroup.alpha = 1f;
        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;
        OnShopStateChanged?.Invoke(shopManager, true);
        isShopOpen = true;
    }

    public void HideShopPanel()
    {
        Time.timeScale = 1f;
        shopCanvasGroup.alpha = 0f;
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;
        OnShopStateChanged?.Invoke(shopManager, false);
        isShopOpen = false;
    }

}

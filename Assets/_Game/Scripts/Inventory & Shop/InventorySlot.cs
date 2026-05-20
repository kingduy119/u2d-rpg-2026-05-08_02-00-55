using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public ItemSO itemSO;
    public int quantity;

    public Image itemImg;
    public TMP_Text quantityText;

    private InventoryManager inventoryManager;
    private static ShopManager activeShop;

    void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
    }

    private void OnEnable()
    {
        ShopManager.OnShopStateChanged += HandleShopStateChanged;
    }

    private void OnDisable()
    {
        ShopManager.OnShopStateChanged -= HandleShopStateChanged;
    }

    void HandleShopStateChanged(ShopManager shopManager, bool isOpen)
    {
        activeShop = isOpen ? shopManager : null;
    }

    public void OnPointerClick(PointerEventData ev)
    {
        if (quantity > 0)
        {
            if (ev.button == PointerEventData.InputButton.Left)
            {
                if (activeShop != null)
                {
                    activeShop.SellItem(itemSO);
                    quantity--;
                    UpdateUI();
                }
                else
                {
                    inventoryManager.UseItem(this);
                }
            }
            else if (ev.button == PointerEventData.InputButton.Right)
            {
                inventoryManager.DropItem(this);
            }
        }
    }

    public void UpdateUI()
    {
        if (quantity <= 0)
            itemSO = null;

        if (itemSO != null)
        {
            itemImg.gameObject.SetActive(true);
            quantityText.gameObject.SetActive(true);

            itemImg.sprite = itemSO.itemIcon;
            quantityText.text = quantity.ToString();
        }
        else
        {
            itemImg.gameObject.SetActive(false);
            quantityText.gameObject.SetActive(false);
        }
    }

}

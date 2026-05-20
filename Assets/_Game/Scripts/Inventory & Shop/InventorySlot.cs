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
    private ShopManager shopManager;
    private static ShopManager activeShop;
    private bool isOpen = false;

    void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
    }

    void OnEnable()
    {
        // ShopManager.OnShopStateChanged += HandleShopStateChanged;
    }

    void OnDisable()
    {
        // ShopManager.OnShopStateChanged -= HandleShopStateChanged;
    }

    void HandleShopStateChanged()
    {
        activeShop = isOpen ? shopManager : null;
    }

    public void OnPointerClick(PointerEventData ev)
    {
        if (quantity > 0)
        {
            if (ev.button == PointerEventData.InputButton.Left)
            {
                inventoryManager.UseItem(this);
            }
            else if (ev.button == PointerEventData.InputButton.Right)
            {
                inventoryManager.DropItem(this);
            }
        }
    }

    public void UpdateUI()
    {
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

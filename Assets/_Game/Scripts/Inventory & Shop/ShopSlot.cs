using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerMoveHandler
{
    public ItemSO itemSO;
    public TMP_Text itemPriceText;
    public Image itemImage;

    [SerializeField] private ShopItemInfo itemInfo;
    [SerializeField] private ShopManager shopManager;
    public int price;


    public void Initalize(ItemSO itemSO, int price)
    {
        this.itemSO = itemSO;
        this.price = price;

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (itemSO == null)
            return;

        itemPriceText.text = price.ToString();
        itemImage.sprite = itemSO.itemIcon;
    }

    public void OnBuyButtonClicked()
    {
        shopManager.BuyItem(itemSO, price);
    }

    public void OnPointerEnter(PointerEventData evData)
    {
        if (itemSO != null)
            itemInfo.ShowItemInfo(itemSO);
    }

    public void OnPointerExit(PointerEventData evData)
    {
        itemInfo.HideItemInfo();
    }

    public void OnPointerMove(PointerEventData evData)
    {
        if (itemSO != null)
            itemInfo.FollowMouse();
    }
}

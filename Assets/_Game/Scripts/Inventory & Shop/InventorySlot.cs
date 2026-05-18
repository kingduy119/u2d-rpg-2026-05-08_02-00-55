using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public ItemSO itemSO;
    public int quantity;

    public Image itemImg;
    public TMP_Text quantityText;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }

    public void OnPointerClick(PointerEventData ev)
    {
        if (quantity > 0)
        {
            if (ev.button == PointerEventData.InputButton.Left)
            {
                inventoryManager.UseItem(this);
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

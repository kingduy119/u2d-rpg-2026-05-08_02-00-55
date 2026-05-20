using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ShopItemInfo : MonoBehaviour
{
    public CanvasGroup infoPanel;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    [Header("Stat Fields")]
    public TMP_Text[] statText;

    private RectTransform infoPanelRect;

    void Awake()
    {
        infoPanelRect = GetComponent<RectTransform>();
    }

    public void ShowItemInfo(ItemSO itemSO)
    {
        infoPanel.alpha = 1;
        nameText.text = itemSO.itemName;
        descriptionText.text = itemSO.itemDescription;
    }

    public void HideItemInfo()
    {
        infoPanel.alpha = 0;
        nameText.text = "";
    }

    public void FollowMouse()
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 offset = new Vector3(90f, -10f, 0f);

        infoPanelRect.position = (Vector3)mousePosition + offset;
    }
}

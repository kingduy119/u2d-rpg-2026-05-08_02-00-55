using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject panel;
    public InventorySlot[] itemSlots;

    void Start()
    {
        foreach (var slot in itemSlots)
        {
            slot.UpdateUI();
        }
        panel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Loot.OnItemLooted += AddItem;

    }
    private void OnDisable()
    {
        Loot.OnItemLooted -= AddItem;
    }

    private void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            panel.gameObject.SetActive(!panel.gameObject.activeSelf);
        }
    }

    public void AddItem(ItemSO item, int quantity)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == null)
            {
                slot.itemSO = item;
                slot.quantity = quantity;
                slot.UpdateUI();
                return;
            }
        }
    }

    public void UseItem(InventorySlot slot)
    {
        if (slot.itemSO != null slot.quantity >= 0) {

        }
    }
}

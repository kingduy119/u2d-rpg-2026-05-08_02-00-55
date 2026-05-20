using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject panel;
    public InventorySlot[] itemSlots;
    private UseItem useItem;
    public GameObject lootPrefab;
    public Transform player;

    public int gold = 0;

    void Start()
    {
        foreach (var slot in itemSlots)
        {
            slot.UpdateUI();
        }
        panel.gameObject.SetActive(false);
        useItem = GetComponent<UseItem>();
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

    public void AddItem(ItemSO itemSO, int quantity)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)
            {
                int availableSpace = itemSO.stackSize - slot.quantity;
                int amountToAdd = Mathf.Min(availableSpace, quantity);

                slot.quantity += amountToAdd;
                quantity -= amountToAdd;

                slot.UpdateUI();
                if (quantity <= 0)
                    return;
            }
        }

        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == null)
            {
                int amountToAdd = Mathf.Min(itemSO.stackSize, quantity);
                slot.itemSO = itemSO;
                slot.quantity = amountToAdd;

                quantity -= amountToAdd;
                slot.UpdateUI();

                if (quantity <= 0)
                    return;
            }
        }

        if (quantity > 0)
            DropLoot(itemSO, quantity);
    }

    public void DropItem(InventorySlot slot)
    {
        DropLoot(slot.itemSO, 1);
        slot.quantity--;
        if (slot.quantity <= 0)
        {
            slot.itemSO = null;
        }
        slot.UpdateUI();
    }

    private void DropLoot(ItemSO itemSO, int quantity)
    {
        Loot loot = Instantiate(lootPrefab, player.position, Quaternion.identity).GetComponent<Loot>();
        loot.Initialize(itemSO, quantity);
    }

    public void UseItem(InventorySlot slot)
    {
        if (slot.itemSO != null && slot.quantity >= 0)
        {
            useItem.ApplyItemEffects(slot.itemSO);
            slot.quantity--;
            slot.UpdateUI();
        }
    }

}

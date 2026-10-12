using System;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer spriteRenderer;
    public Animator anim;

    public int quantity = 0;
    private bool isPicked = false;
    private bool canPickUp = true;

    public static event Action<ItemSO, int> OnItemLooted;

    private void OnValidate()
    {
        if (itemSO == null)
            return;

        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        spriteRenderer.sprite = itemSO.itemIcon;
        // this.name = itemSO.itemName;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPicked) return;

        if (collision.CompareTag("Player") && canPickUp)
        {
            anim.Play("item_pickup");
            OnItemLooted?.Invoke(itemSO, quantity);
            canPickUp = false;
            isPicked = true;
            Hidden();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickUp = true;
        }
    }

    public void Hidden()
    {
        Destroy(gameObject, .3f);
    }

    public void Initialize(ItemSO itemSO, int quantity)
    {
        this.itemSO = itemSO;
        this.quantity = quantity;
        canPickUp = false;
        UpdateAppearance();
    }
}

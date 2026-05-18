using System;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer spriteRenderer;
    public Animator anim;

    public int quantity = 0;

    public static event Action<ItemSO, int> OnItemLooted;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnValidate()
    {
        if (itemSO != null)
        {
            spriteRenderer.sprite = itemSO.itemIcon;
            this.name = itemSO.itemName;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("item_pickup");
            anim.Play("item_pickup");
            OnItemLooted?.Invoke(itemSO, quantity);
        }
    }

    public void Hidden()
    {
        Destroy(gameObject);
    }
}

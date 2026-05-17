using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer spriteRenderer;
    public Animator anim;

    public int quantity = 0;

    private void OnValidate()
    {
        if (itemSO != null)
        {
            spriteRenderer.sprite = itemSO.itemIcon;
            this.name = itemSO.itemName;
        }
    }

}

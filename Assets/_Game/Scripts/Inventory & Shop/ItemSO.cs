using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite itemIcon;

    public string type;
    public int value;
    public int maxValue;

    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int speed;
    public int damage;

    [Header("For Temporary Items")]
    public float duration;
}

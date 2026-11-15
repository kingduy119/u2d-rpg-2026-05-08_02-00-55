
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDTO", menuName = "DTO/ItemDTO")]
public class ItemDTO : ScriptableObject
{
    public ItemType Type;
    public Sprite Icon;

    [Header("Stack")]
    public int MaxStack;
}

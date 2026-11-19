

using UnityEngine;

namespace Inventory
{
    public enum ItemType
    {
        Gold,
        Diamond,
        Currency
    }

    [CreateAssetMenu(fileName = "ItemDto", menuName = "Inventory/ItemDto")]
    public class ItemDto : ScriptableObject
    {
        public ItemType Type;
        public Sprite Icon;

        [Header("Stack")]
        public int MaxStack;
    }
}

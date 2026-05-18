using UnityEngine;

public class UseItem : MonoBehaviour
{
    public void ApplyItemEffects(ItemSO itemSO)
    {
        if (itemSO.health > 0)
            StateManager.Instance.UpdateHealth(itemSO.health);
        if (itemSO.maxHealth > 0)
            StateManager.Instance.UpdateHealth(itemSO.maxHealth);
        if (itemSO.speed > 0)
            StateManager.Instance.speed = itemSO.speed;
    }
}

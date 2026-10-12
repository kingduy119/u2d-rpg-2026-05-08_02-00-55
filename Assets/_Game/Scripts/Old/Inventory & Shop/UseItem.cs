using UnityEngine;
using System.Collections;

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

        if (itemSO.duration > 0)
            StartCoroutine(EffectTimer(itemSO, itemSO.duration));
    }

    private IEnumerator EffectTimer(ItemSO itemSO, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (itemSO.health > 0)
            StateManager.Instance.UpdateHealth(-itemSO.health);

        if (itemSO.maxHealth > 0)
            StateManager.Instance.UpdateHealth(-itemSO.maxHealth);

        if (itemSO.speed > 0)
            StateManager.Instance.UpdateHealth(-itemSO.speed);
    }
}

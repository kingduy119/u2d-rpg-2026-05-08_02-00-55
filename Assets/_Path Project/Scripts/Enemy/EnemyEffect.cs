using UnityEngine;


public interface IEffectTrigger
{
    void TriggerEffect();
}

public interface IDamageable
{
    void TakeDamage(float amount);
}

namespace TDGame
{
    public class EnemyEffect : MonoBehaviour, IEffectTrigger
    {
        [SerializeField] private GameObject m_EffectPrefab;
        public void TriggerEffect()
        {
            Instantiate(m_EffectPrefab, transform.position, Quaternion.identity);
        }
    }
}
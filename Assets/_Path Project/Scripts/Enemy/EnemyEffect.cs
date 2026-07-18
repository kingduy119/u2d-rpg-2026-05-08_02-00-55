using UnityEngine;

namespace TDGame
{
    public class EnemyEffect : MonoBehaviour, IEffectTrigger
    {
        [SerializeField] private GameObject m_EffectPrefab;

        private EnemyHealth m_Health;

        public void TriggerEffect()
        {
            Instantiate(m_EffectPrefab, transform.position, Quaternion.identity);
        }
    }
}


using UnityEngine;

namespace Characters
{
    public class CharacterDraw : MonoBehaviour
    {
        public bool ShowRadius;
        public Transform AttackPoint;
        public float AttackRange;

        private void OnDrawGizmosSelected()
        {
            if (!ShowRadius) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);

        }
    }
}
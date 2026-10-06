

using UnityEngine;

namespace Characters
{
    public class CharacterDraw : MonoBehaviour
    {
        public bool ShowRadius;
        public Character character;
        public Transform AttackPoint;


        private void OnDrawGizmosSelected()
        {
            if (!ShowRadius) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(AttackPoint.transform.position, character.SO.Combat.AttackRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, character.SO.Combat.DetectionRange);

        }
    }
}
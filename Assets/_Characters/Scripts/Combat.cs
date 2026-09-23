

using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Animator))]
    public class Combat : MonoBehaviour
    {
        private Animator _animator;

        public LayerMask enemyLayer;
        public Transform attackPoint;
        public Transform shootPoint;

        private CombatData Data;
        public void SetData(CharacterSO SO) => Data = SO.Combat;

        public float AttackCoolDown { get; private set; } = 0f;
        public float WeaponRange { get; private set; } = 0.5f;
        public bool Attacking { get; private set; }

        public bool CanAttack => AttackCoolDown <= 0 && !Attacking;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (AttackCoolDown > 0)
                AttackCoolDown -= Time.deltaTime;
        }

        public void Attack_Start()
        {
            Attacking = true;
        }

        public void Attack_Done()
        {
            Attacking = false;
            AttackCoolDown = Data.AttackSpeed;
        }

        public void Deal_Damge()
        {

        }
    }
}

using UnityEngine;

namespace Characters
{

    public class Character : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected Combat _Combat;
        [SerializeField] protected CharacterSO Data;

        public Rigidbody2D Rb => _rigidbody;
        public Animator Anim => _animator;
        public Combat Combat => _Combat;
        public CharacterSO ShareData => Data;

        public Vector2 Direction { get; private set; }
        public Transform AttackPoint;

        public StateMachine States = new();
        public IState nextState;
        public IState idleState;
        public IState moveState;
        public IState combatState;

        protected virtual void Start()
        {
            _Combat.SetData(Data);

            idleState = new IdleState(this);
            moveState = new MovementSate(this);
            combatState = new CombatState(this);

            nextState = idleState;
            States.Initialize(idleState);
        }

        protected virtual void FixedUpdate()
        {
            States.Execute();
        }

        public void Idle()
        {
            nextState = idleState;
            Direction = Vector2.zero;
        }

        public void Move(Vector2 input)
        {
            if (nextState is CombatState) return;

            Direction = input;
            if (Direction == Vector2.zero)
                nextState = idleState;
            else
                nextState = moveState;
        }

        public void Attack()
        {
            if (!Combat.Attacking)
                nextState = combatState;
        }

        public void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

}

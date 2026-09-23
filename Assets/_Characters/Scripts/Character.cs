using System.Collections.Generic;
using UnityEngine;

namespace Characters
{

    public class Character : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected CharacterSO Data;

        public LayerMask TargetLayer { get; private set; }
        public void SetTargetLayer(LayerMask layer) => TargetLayer = layer;

        public Rigidbody2D Rb => _rigidbody;
        public Animator Anim => _animator;
        public CharacterSO ShareData => Data;

        public Vector2 Direction { get; private set; }
        public Transform AttackPoint;

        public StateMachine States = new();
        public ICterState nextState;
        public ICterState idleState;
        public ICterState moveState;
        public ICterState combatState;

        private List<ICterState> _states = new();

        protected virtual void Start()
        {
            idleState = new IdleState(this);
            moveState = new MovementSate(this);
            combatState = new CombatState(this);

            _states.Add(combatState);

            nextState = idleState;
            States.Initialize(idleState);
        }

        protected virtual void Update()
        {
            foreach (var state in _states)
            {
                state.Tick(Time.deltaTime);
            }
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
            if (States.CurrentState is CombatState || nextState is CombatState) return;

            Direction = input;
            if (Direction == Vector2.zero)
                nextState = idleState;
            else
                nextState = moveState;
        }

        public void Attack()
        {
            // if (!Combat.Attacking)
            if (States.CurrentState is not CombatState)
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

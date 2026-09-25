using System.Collections.Generic;
using UnityEngine;

public enum Colors
{
    Blue,
    Red,
}

namespace Characters
{

    public interface ICterAttribute
    {
        Transform GetAttackPoint();
        CharacterSO GetData();
    }

    public interface ICter : ICterAttribute
    {
        void Idle();
        void Move(Vector2 input);
        void Attack();
        void Flip();
    }

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterColor))]
    public class Character : MonoBehaviour,
        ICter
    {
        [SerializeField] protected CharacterSO Data;

        public LayerMask TargetLayer { get; private set; }
        public void SetTargetLayer(LayerMask layer) => TargetLayer = layer;

        public Rigidbody2D Rb { get; private set; }
        public Animator Anim { get; private set; }
        public CharacterSO ShareData => Data;
        public CharacterSO GetData() => Data;

        public Vector2 Direction { get; private set; }
        public Transform AttackPoint;
        public Transform GetAttackPoint() => AttackPoint;


        public StateMachine States = new();
        public ICterState nextState;
        public ICterState idleState;
        public ICterState moveState;
        public ICterState combatState;

        private List<ICterState> _states = new();

        protected virtual void Awake()
        {
            Anim = GetComponent<Animator>();
            Rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            idleState = new IdleState(this);
            moveState = new MovementSate(this);
            combatState = new CombatState(this);

            nextState = idleState;
            States.Initialize(idleState);

            _states.Add(combatState); // For state can call Tick() in Update 
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

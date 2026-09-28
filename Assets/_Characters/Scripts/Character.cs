using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum Colors
{
    Blue,
    Red,
}
namespace Characters
{

    public interface ICterAction
    {
        void Idle();
        void Move(Vector2 input);
        void Attack(Transform target);
    }

    public interface ICter : ICterAbstract, ICterAction { }

    [RequireComponent(typeof(CharacterColor))]
    public class Character : CharacterAbstract,
    ICter,
    IPoolable<Character>
    {
        public IObjectPool<Character> Pool { get; set; }
        public LayerMask TargetLayer { get; private set; }
        public void SetTargetLayer(LayerMask layer) => TargetLayer = layer;

        public Vector2 Direction { get; private set; }

        public StateMachine States = new();
        public ICterState nextState;
        public ICterState idleState;
        public ICterState moveState;
        public ICterState combatState;

        public CharacterSO SO => Data;

        protected readonly List<ICterState> _states = new();

        protected virtual void Start()
        {
            InitStates();

            nextState = idleState;
            States.Initialize(idleState);
        }

        protected virtual void InitStates()
        {
            idleState = new IdleState(this);
            moveState = new MovementSate(this);
            combatState = new CombatState(this);

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

        public void Attack(Transform target)
        {
            Target = target;
            if (States.CurrentState is not CombatState)
                nextState = combatState;
        }

    }

}

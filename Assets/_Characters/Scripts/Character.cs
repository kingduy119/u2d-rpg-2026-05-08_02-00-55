using UnityEngine;

namespace Characters
{

    public class Character : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigidbody;
        [SerializeField] Animator _animator;
        [SerializeField] CharacterSO Data;

        [SerializeField] Combat _Combat;
        [SerializeField] Movement _Movement;

        public Animator Anim => _animator;
        public Rigidbody2D Rb => _rigidbody;
        public Transform AttackPoint;
        public CharacterSO ShareData => Data;
        public Vector2 Direction { get; private set; }

        public Combat Combat => _Combat;


        public StateMachine States = new();
        public IState nextState;
        public IState idleState;
        public IState moveState;
        public IState combatState;

        private void Start()
        {
            _Movement.SetData(Data);
            _Combat.SetData(Data);

            idleState = new IdleState(this);
            moveState = new MovementSate(this);
            combatState = new CombatState(this);

            nextState = idleState;
            States.Initialize(idleState);
        }

        private void FixedUpdate()
        {
            States.Execute();
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
            nextState = combatState;
        }

        public void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public class IdleState : IState
    {
        private Character _Character;
        public IdleState(Character character)
        {
            _Character = character;
        }

        public void Enter()
        {
            Debug.Log("Idle enter");
        }

        public void Execute()
        {
            if (_Character.nextState is not IdleState)
                _Character.States.TransitionTo(_Character.nextState);
        }

        public void Exit()
        {
        }
    }

    public class MovementSate : IState
    {
        private readonly Character _Character;
        private MoveData Data;
        public MovementSate(Character character)
        {
            _Character = character;
            Data = _Character.ShareData.Move;
        }

        public void Execute()
        {
            Move(_Character.Direction);

            if (_Character.nextState is CombatState)
            {
                _Character.States.TransitionTo(_Character.combatState);
            }
        }

        public void Exit()
        {
            Stop();
        }

        private void Move(Vector2 input)
        {
            Vector2 direction = input.normalized;
            _Character.Rb.linearVelocity = direction * Data.Speed;

            _Character.Anim.SetFloat("moveX", Mathf.Abs(direction.x));
            _Character.Anim.SetFloat("moveY", Mathf.Abs(direction.y));
            _Character.Anim.SetFloat("Speed", _Character.Rb.linearVelocity.magnitude);

            if (direction.x > 0 && _Character.transform.localScale.x < 0 ||
                direction.x < 0 && _Character.transform.localScale.x > 0)
            {
                _Character.Flip();
            }
        }

        private void Stop()
        {
            _Character.Rb.linearVelocity = Vector2.zero;
            _Character.Anim.SetFloat("Speed", _Character.Rb.linearVelocity.magnitude);
        }
    }


    public class CombatState : IState
    {
        private readonly Character _Character;
        public CombatState(Character character)
        {
            _Character = character;
        }

        public void Enter()
        {
            _Character.Combat.Attack();
        }

        public void Execute()
        {
            if (!_Character.Combat.Attacking)
            {
                _Character.nextState = _Character.idleState;
                _Character.States.TransitionTo(_Character.nextState);
            }
        }
    }

}

using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Characters
{
    public abstract class CharacterState : IState
    {
        protected readonly Character _Character;
        public CharacterState(Character character)
        {
            _Character = character;
        }

        public virtual void Execute() { }
        public virtual void Enter() { }
        public virtual void Exit() { }
    }

    public class IdleState : CharacterState
    {
        public IdleState(Character character) : base(character) { }
        public override void Execute()
        {
            if (_Character.nextState is not IdleState)
                _Character.States.TransitionTo(_Character.nextState);
        }
    }

    public class MovementSate : CharacterState
    {
        private readonly MoveData Data;
        private static readonly int RunSpeedHash = Animator.StringToHash("RunSpeed");
        public MovementSate(Character character) : base(character)
        {
            Data = _Character.ShareData.Move;
        }

        public override void Execute()
        {
            if (_Character.Combat.Attacking) return;

            HandleMovement(_Character.Direction);

            if (_Character.nextState is CombatState)
                _Character.States.TransitionTo(_Character.combatState);
        }

        public override void Exit()
        {
            // Stop
            _Character.Rb.linearVelocity = Vector2.zero;
            _Character.Anim.SetFloat(RunSpeedHash, _Character.Rb.linearVelocity.magnitude);
        }

        private void HandleMovement(Vector2 input)
        {
            Vector2 direction = input.normalized;
            _Character.Rb.linearVelocity = direction * Data.Speed;
            _Character.Anim.SetFloat(RunSpeedHash, _Character.Rb.linearVelocity.magnitude);

            if (direction.x > 0 && _Character.transform.localScale.x < 0 ||
                direction.x < 0 && _Character.transform.localScale.x > 0)
            {
                _Character.Flip();
            }
        }
    }

    public class CombatState : CharacterState
    {
        private static readonly int IsAttacking1Hash = Animator.StringToHash("isAttacking1");
        public CombatState(Character character) : base(character) { }

        public override void Enter()
        {
            Attack().Forget();
        }

        public override void Execute()
        {
            if (!_Character.Combat.Attacking)
            {
                _Character.nextState = _Character.idleState;
                _Character.States.TransitionTo(_Character.nextState);
            }
        }

        public override void Exit()
        {
            _Character.Anim.SetBool(IsAttacking1Hash, false);
        }

        private async UniTask Attack()
        {
            if (!_Character.Combat.CanAttack) return;

            _Character.Combat.Attack_Start();
            _Character.Anim.SetBool(IsAttacking1Hash, true);

            await UniTask.Delay(200);
            _Character.Combat.Deal_Damge();

            await UniTask.Delay(100);
            _Character.Combat.Attack_Done();

        }
    }

    public class PatrolState : CharacterState
    {
        public PatrolState(Character character) : base(character)
        {
        }
    }
}
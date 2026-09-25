using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Characters
{
    public interface ICterState : IState
    {
        void Tick(float deltaTime);
    }

    public abstract class CharacterState : ICterState
    {
        protected readonly Character _Character;
        public CharacterState(Character character)
        {
            _Character = character;
        }

        public virtual void Execute() { }
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick(float deltaTime) { }
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
        private CombatData _combatData;
        private bool _attacking;
        private float _attackCooldown = 0f;
        private bool AttackActive => _attackCooldown <= 0 && !_attacking;


        public CombatState(Character character) : base(character)
        {
            _combatData = character.ShareData.Combat;
        }

        public override void Enter()
        {
            Attack().Forget();
        }

        public override void Execute()
        {
            if (AttackActive)
            {
                _Character.nextState = _Character.idleState;
                _Character.States.TransitionTo(_Character.nextState);
            }
        }

        public override void Tick(float deltaTime)
        {
            if (_attackCooldown > 0) _attackCooldown -= deltaTime;
        }

        private async UniTask Attack()
        {
            if (!AttackActive) return;

            _attacking = true;
            _Character.Anim.SetBool(IsAttacking1Hash, true);

            await UniTask.Delay(200);
            CheckAttackCollision2D();

            await UniTask.Delay(100);
            _Character.Anim.SetBool(IsAttacking1Hash, false);

            await UniTask.Delay(200);
            _attacking = false;
            _attackCooldown = _combatData.AttackSpeed;
        }

        private void CheckAttackCollision2D()
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(
                _Character.AttackPoint.position,
                _combatData.AttackRange,
                _Character.TargetLayer
            );

            if (targets.Length > 0)
            {
                Debug.Log("CheckAttackCollision2D");
            }
        }

    }
}
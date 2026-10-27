
using Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterCombat : MonoBehaviour, IState
{

    Character _Character;
    protected bool _attacking;
    protected float _attackCooldown = 0f;
    protected float _attackSpeed = 2f;
    protected float _attackRange = 2f;
    protected float _attackDamage = 2f;
    protected bool AttackActive => _attackCooldown <= 0 && !_attacking;
    protected static readonly int IsAttacking1Hash = Animator.StringToHash("isAttacking1");

    protected CombatData _combatData;

    public void Init(Character character)
    {
        _Character = character;
        _combatData = character.SO.Combat;
    }
    public void Enter()
    {
        Attack().Forget();
    }

    public void Execute()
    {
        if (AttackActive)
        {
            _Character.nextState = _Character.idleState;
            _Character.States.TransitionTo(_Character.nextState);
        }
    }

    public void Exit()
    { }

    protected virtual async UniTask Attack()
    {
        if (!AttackActive) return;

        _attacking = true;
        _Character.Anim.SetBool(IsAttacking1Hash, true);

        await UniTask.Delay(200);
        CheckAttackCollision2D();

        await UniTask.Delay(100);
        _Character.Anim.SetBool(IsAttacking1Hash, false);

        // await UniTask.Delay(200);
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
            DamageableHelper.CheckCollisionInterfaces(targets[0].gameObject, _combatData.AttackDamage);
        }
    }

}
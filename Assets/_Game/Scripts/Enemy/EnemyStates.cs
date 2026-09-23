using UnityEngine;
using Characters;
using System.Collections;

public class EnemyState : State
{
    protected EnemyController2 _EnemyCtl;
    public EnemyState(EnemyController2 control)
    {
        _EnemyCtl = control;
    }
}


public class EnemyChaseState : EnemyState
{
    readonly Character _character;
    Vector3 _startPos;
    bool _GoingBack;

    public EnemyChaseState(EnemyController2 control) : base(control)
    {
        _character = _EnemyCtl.Character;
        _startPos = _EnemyCtl.StartPosition.position;
    }

    public override void Enter()
    {
        Debug.Log("EnemeyChaseState.Enter");
    }

    public override void Execute()
    {
        if (_EnemyCtl.Target != null)
        {
            if (HasTargetInAttackRange())
                _character.Attack();
            else
            {
                Vector2 direction = _EnemyCtl.Target.position - _EnemyCtl.transform.position;
                _character.Move(direction);
            }
            return;
        }

        float distance = Vector2.Distance(_startPos, _EnemyCtl.transform.position);
        if (distance > 3f || _GoingBack)
        {
            Vector2 direction = _startPos - _EnemyCtl.transform.position;
            _character.Move(direction);
            _GoingBack = true;

            if (distance < .1f) _GoingBack = false;

            return;
        }

        _EnemyCtl.States.TransitionTo(_EnemyCtl.WanderState);
    }

    public override void Exit()
    {
        _character.Idle();
    }

    private bool HasTargetInAttackRange()
    {
        float detectRange = _character.ShareData.Combat.AttackRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            _character.AttackPoint.position,
            detectRange,
            _EnemyCtl.TargetLayer);

        if (colliders.Length > 0)
        {
            Transform target = colliders[0].transform;
            float distance = Vector2.Distance(_EnemyCtl.transform.position, target.position);
            return distance <= detectRange;
        }
        return false;
    }

}

public class EnemyWanderState : EnemyState
{
    private bool _Paused;
    private float _PauseDuration = 2f;
    private float width = 6f;
    private float height = 6f;
    private Vector3 target;
    private Vector3 _startPost;

    public EnemyWanderState(EnemyController2 control) : base(control)
    {
        _startPost = _EnemyCtl.StartPosition.position;
    }

    public override void Enter()
    {
        _EnemyCtl.StartCoroutine(PauseAndPickNewDestination());
    }

    public override void Execute()
    {
        if (_Paused) return;

        Vector3 position = _EnemyCtl.transform.position;
        Vector2 offset = target - position;
        if (offset.sqrMagnitude < .1f) // distance
        {
            _EnemyCtl.StartCoroutine(PauseAndPickNewDestination());
            return;
        }

        offset = target - position;
        _EnemyCtl.Character.Move(offset);

        if (_EnemyCtl.Target) _EnemyCtl.States.TransitionTo(_EnemyCtl.ChaseState);
    }

    private IEnumerator PauseAndPickNewDestination()
    {
        _Paused = true;
        _EnemyCtl.Character.Idle();
        yield return new WaitForSeconds(_PauseDuration);

        target = GetRandomTarget();
        _Paused = false;
    }

    private Vector3 GetRandomTarget()
    {
        float W = (width - 1) / 2;
        float H = (height - 1) / 2;
        float randomX = Random.Range(_startPost.x - W, _startPost.x + W) + 1;
        float randomY = Random.Range(_startPost.y - H, _startPost.y + H) + 1;
        return new Vector3(randomX, randomY);
    }
}
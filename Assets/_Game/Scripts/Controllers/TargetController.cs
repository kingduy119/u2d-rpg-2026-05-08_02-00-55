using System.Collections.Generic;
using Characters;
using UnityEngine;

public interface ITargetController
{
    void Init(string layer, LayerMask targetLayer, Transform defaultTarget = null);
    void Init(Vector3 position, string layer, string targetLayer);
    void SetTargets(List<TargetController> targets);
    void MoveByPointer(Vector3 position);
    void BackToPosition();

    void StartCombat();
    void Reset();

    void SetPosition(Vector3 position);
    Vector3 GetPosition();
    Vector3 GetPrevPosition();
}

public class TargetController : MonoBehaviour, ITargetController
{
    public LayerMask TargetLayer;
    public Transform Target;
    public List<TargetController> Targets = new();
    public Transform DefaultTarget;

    private bool _start = false;
    private float _targetDetect;
    private float _attackRange;
    private Vector3 _prevPosition;

    private Rigidbody2D _rb;

    public ICter Character { get; private set; }

    public bool isDebug;

    public void MoveByPointer(Vector3 position)
    {
        if (_rb != null)
        {
            _rb.MovePosition(position);
            return;
        }
        gameObject.transform.position = position;
    }

    private void Awake()
    {
        if (TryGetComponent<Character>(out var cter))
        {
            Character = cter;
            _targetDetect = cter.SO.Combat.DetectionRange;
            _attackRange = cter.SO.Combat.AttackRange;
        }
    }

    private void Update()
    {
        if (_start) HandleCharacter();
    }

    public void SetTargets(List<TargetController> targets) => Targets = targets;

    public void BackToPosition() => transform.position = _prevPosition;
    public Vector3 GetPrevPosition() => _prevPosition;
    public Vector3 GetPosition() => transform.position;
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        _prevPosition = position;
    }

    private void HandleCharacter()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            Character.AttackPoint.position,
            _attackRange,
            TargetLayer);

        if (isDebug) Debug.Log($"colliders: {colliders.Length}");

        if (colliders.Length > 0)
        {
            var (target, distance) = GetNearestTarget(colliders);

            if (isDebug) Debug.Log($"distance <= _attackRange: {distance} - {_attackRange}");

            if (distance <= _attackRange)
            {
                Character.Attack(target);
            }
        }
        else if (Targets.Count > 0)
        {
            var (nearestTarget, distance) = GetNearestTarget(Targets);
            if (isDebug) Debug.Log($"GetNearestTarget: {distance} - {_attackRange}");
            if (distance > _attackRange)
            {
                Vector2 direction = nearestTarget.GetPosition() - transform.position;
                Character.Move(direction.normalized);
            }
        }
        else Character.Idle();
    }

    // private bool IsAttacking()
    // {
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(
    //         Character.AttackPoint.position,
    //         _attackRange,
    //         TargetLayer);

    //     if (colliders.Length > 0)
    //     {
    //         var (target, distance) = GetNearestTarget(colliders);
    //         if (distance <= _attackRange)
    //         {
    //             Character.Attack(target);
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    private (TargetController nearest, float distance) GetNearestTarget(List<TargetController> targets)
    {
        // Transform nearest = null;
        TargetController nearest = null;
        float minDistance = float.MaxValue;

        foreach (var target in targets)
        {
            if (target == null) continue;

            float distance = Vector2.Distance(
                transform.position,
                target.GetPosition()
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = target;
            }
        }

        return (nearest, minDistance);
    }

    private (Transform nearest, float distance) GetNearestTarget(Collider2D[] colliders)
    {
        // Transform nearest = null;
        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (var target in colliders)
        {
            if (target == null) continue;

            float distance = Vector2.Distance(
                transform.position,
                target.transform.position
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = target.transform;
            }
        }

        return (nearest, minDistance);
    }


    public void StartCombat()
    {
        _start = true;
    }

    public void Reset()
    {
        _start = false;
        transform.position = _prevPosition;
    }

    public void Init(string layer, LayerMask targetLayer, Transform defaultTarget = null)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
        TargetLayer = targetLayer;
        DefaultTarget = defaultTarget;

        Character.SetTargetLayer(TargetLayer);
    }

    public void Init(Vector3 position, string layer, string targetLayer)
    {
        if (position != null) SetPosition(position);

        gameObject.layer = LayerMask.NameToLayer(layer);
        TargetLayer = LayerMask.GetMask(targetLayer);

        Character.SetTargetLayer(TargetLayer);
    }


}



// private ITargetController GetNearestTarget()
// {
//     ITargetController nearest = null;
//     float minDistance = float.MaxValue;

//     foreach (var target in Targets)
//     {
//         if (target == null) continue;

//         float distance = Vector2.Distance(
//             transform.position,
//             target.GetPosition()
//         );

//         if (distance < minDistance)
//         {
//             minDistance = distance;
//             nearest = target;
//         }
//     }

//     return nearest;
// }

// private void CheckTargetInRange()
// {
//     var colider = Physics2D.OverlapCircle(transform.position, _targetDetect, TargetLayer);
//     Target = colider ? colider.transform : null;
// }
using System.Collections.Generic;
using Characters;
using UnityEngine;
using System.Linq;


public interface ITargetController
{
    void Init(string layer, LayerMask targetLayer, Transform defaultTarget = null);
    void Init(Vector3 position, string layer, string targetLayer);
    void SetTargets(List<TargetController> targets);
    void MoveByPointer(Vector3 position);
    void BackToPosition();

    // void StartCombat();
    // void Reset();

    void SetPosition(Vector3 position);
    Vector3 GetPosition();
    Vector3 GetPrevPosition();
}

public class TargetController : MonoBehaviour, ITargetController
{
    public LayerMask TargetLayer;
    public Transform Target;
    public List<Transform> Targets = new();
    public Transform DefaultTarget;

    private bool _start = false;
    private float _targetDetect;
    private float _attackRange;
    private Vector3 _prevPosition;
    public Vector3Int Cell;

    private Rigidbody2D _rb;

    public Character Character { get; private set; }

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

    public void Swap(TargetController other)
    {
        (Cell, other.Cell) = (other.Cell, Cell);
    }

    public void SetTargets(List<Transform> targets) => Targets = targets;
    public void SetTargets(List<TargetController> targets) => Targets = targets.Select(t => t.transform).ToList();
    public void BackToPosition() => transform.position = _prevPosition;
    public Vector3 GetPrevPosition() => _prevPosition;
    public Vector3 GetPosition() => transform.position;
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        _prevPosition = position;
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
        // if (_start) 
        HandleCharacter();
    }

    private void HandleCharacter()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            Character.AttackPoint.position,
            _attackRange,
            TargetLayer);

        if (colliders.Length > 0)
        {
            var (target, distance) = GetNearestTarget(colliders);

            if (distance <= _attackRange)
            {
                Character.Attack(target);
            }
        }
        else if (Targets.Count > 0)
        {
            var (nearestTarget, distance) = GetNearestTarget(Targets);

            if (nearestTarget != null && distance > _attackRange)
            {
                Vector2 direction = nearestTarget.position - transform.position;
                Character.Move(direction.normalized);
            }
        }
        else Character.Idle();
    }

    private (Transform nearest, float distance) GetNearestTarget(List<Transform> targets)
    {
        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (var target in targets)
        {
            if (target == null) continue;

            float distance = Vector2.Distance(
                transform.position,
                target.position
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


    // public void StartCombat()
    // {
    //     _start = true;
    // }
    public void Deactive()
    {
        // _start = false;
        gameObject.tag = "Untagged";
        this.enabled = false;
        Character.Deactive();
    }

    public void Init(string layer, LayerMask targetLayer, Transform defaultTarget = null)
    {
        gameObject.tag = layer;
        gameObject.layer = LayerMask.NameToLayer(layer);
        TargetLayer = targetLayer;
        DefaultTarget = defaultTarget;

        Character.SetTargetLayer(TargetLayer);
    }

    public void Init(Vector3 position, string layer, string targetLayer)
    {
        if (position != null) SetPosition(position);

        gameObject.tag = layer;
        gameObject.layer = LayerMask.NameToLayer(layer);
        TargetLayer = LayerMask.GetMask(targetLayer);

        Character.SetTargetLayer(TargetLayer);
    }

    public void Init(string layer, string targetLayer)
    {
        gameObject.tag = layer;
        gameObject.layer = LayerMask.NameToLayer(layer);
        TargetLayer = LayerMask.GetMask(targetLayer);

        Character.SetTargetLayer(TargetLayer);
    }


}


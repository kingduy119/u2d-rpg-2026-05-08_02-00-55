using Characters;
using UnityEngine;

public interface ITargetController
{
    void Init(string layer, LayerMask targetLayer, Transform defaultTarget = null);
    void Init(Vector3 position, string layer, string targetLayer);
    void MoveByPointer(Vector3 position);
    void BackToPosition();

    void Start();
    void Reset();

    void SetPosition(Vector3 position);
    Vector3 GetPosition();
    Vector3 GetPrevPosition();
}

public class TargetController : MonoBehaviour, ITargetController
{
    public LayerMask TargetLayer;
    public Transform Target;
    public Transform DefaultTarget;

    private bool _start = false;
    private float _targetDetect;
    private float _attackRange;
    private Vector3 _prevPosition;

    private Rigidbody2D _rb;

    public ICter Character { get; private set; }

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
        // if (TryGetComponent<Rigidbody2D>(out var rb)) _rb = rb;
    }

    private void Update()
    {
        if (!_start) return;

        CheckTargetInRange();
        HandleCharacter();
    }

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
        if (Target && CanAttack())
        {
            Character.Attack(Target);
            return;
        }

        Transform target = Target ? Target : DefaultTarget;
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance > 0.1f)
        {
            Vector2 direction = target.position - transform.position;
            Character.Move(direction);
        }
        else Character.Idle();

    }

    private void CheckTargetInRange()
    {
        var colider = Physics2D.OverlapCircle(transform.position, _targetDetect, TargetLayer);
        Target = colider ? colider.transform : null;
    }

    private bool CanAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            Character.AttackPoint.position,
            _attackRange,
            TargetLayer);

        if (colliders.Length > 0)
        {
            Transform target = colliders[0].transform;
            float distance = Vector2.Distance(Target.transform.position, target.position);
            return distance <= _attackRange;
        }
        return false;
    }


    public void Start()
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
        TargetLayer = LayerMask.NameToLayer(targetLayer);
        Character.SetTargetLayer(TargetLayer);
    }
}




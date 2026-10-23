using Characters;
using UnityEngine;

public interface ITargetController
{
    void Init(string layer, LayerMask targetLayer, Transform defaultTarget = null);
    void MoveByPointer(Vector3 position);

}

public class TargetController : MonoBehaviour, ITargetController
{
    public LayerMask TargetLayer;
    public Transform Target;
    public Transform DefaultTarget;
    private float _targetDetect;
    private float _attackRange;

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
        if (!GameDefenseRuleView.Started) return;

        CheckTargetInRange();
        HandleCharacter();
    }

    private void HandleCharacter()
    {
        if (Target && CanAttack())
        {
            Character.Attack(Target);
            return;
        }

        Transform target = Target ? Target : DefaultTarget;
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

    public void Init(string layer, LayerMask targetLayer, Transform defaultTarget = null)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
        TargetLayer = targetLayer;
        DefaultTarget = defaultTarget;
        Character.SetTargetLayer(targetLayer);
    }
}




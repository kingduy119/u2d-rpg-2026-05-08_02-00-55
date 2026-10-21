using Characters;
using UnityEngine;

public interface ITargetController
{
    void Init(string layer, LayerMask targetLayer, Transform defaultTarget = null);
}

public class TargetController : MonoBehaviour, ITargetController
{
    public LayerMask TargetLayer;
    public Transform Target;
    public Transform DefaultTarget;
    private float _targetDetect;
    private float _attackRange;

    public ICter Character { get; private set; }

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



// {
// [SerializeField] private Animator m_animator;

// public Transform Target { get; set; }
// public Vector2 Direction { get; private set; }
// private EnemyStateMachine m_stateMachine;

// public EnemyStateMachine StateMachine => m_stateMachine;
// public Animator Animator => m_animator;
// public EnemyMovement Movement { get; private set; }
// public EnemyCombat Combat { get; private set; }

// private void Awake()
// {
//     m_stateMachine = new(this);

//     Movement = GetComponent<EnemyMovement>();
//     Combat = GetComponent<EnemyCombat>();

//     Movement.Initialize(this);
//     Combat.Initialize(this);
// }

// private void Start()
// {
//     m_stateMachine.Initialize(m_stateMachine.m_idleState);
// }

// private void Update()
// {
//     m_stateMachine.Execute();
//     Combat.CheckPlayerInAttackRange();

//     CheckTargetInRange();
// }

// private void OnTriggerEnter2D(Collider2D collision)
// {
//     if (collision.CompareTag("Player"))
//     {
//         Target = collision.transform;
//     }
// }

// private void OnTriggerExit2D(Collider2D collision)
// {
//     if (collision.CompareTag("Player"))
//     {
//         Target = null;
//     }
// }

// private void CheckTargetInRange()
// {
//     if (Target == null)
//         return;

//     Direction = (Target.position - transform.position).normalized;
//     if (Direction.x > 0 && transform.localScale.x < 0 ||
//    Direction.x < 0 && transform.localScale.x > 0)
//     {
//         Flip();
//     }
// }

// private void Flip()
// {
//     Vector3 scale = transform.localScale;
//     scale.x *= -1;
//     transform.localScale = scale;
// }
// }

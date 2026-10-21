using Characters;
using UnityEngine;

public interface ITargetController
{
    // void SetTarget(Vector3 position);
    void Init(LayerMask layer, Transform defaultTarget = null);
}

public class TargetController : MonoBehaviour, ITargetController
{
    public LayerMask TargetLayer;
    public Transform Target;
    public Transform DefaultTarget;
    private float _rangerDetect;

    public ICter Character { get; private set; }

    private void Awake()
    {
        if (TryGetComponent<Character>(out var character))
        {
            character.SetTargetLayer(TargetLayer);
            Character = character;
        }
    }

    private void Update()
    {
        Collider2DTrigger();
        HandleCharacter();
    }

    private void HandleCharacter()
    {
        if (Target)
        {
            Character.Attack(Target);
            return;
        }

        float distance = Vector2.Distance(transform.position, DefaultTarget.position);
        if (distance > 0.1f)
        {
            Vector2 direction = DefaultTarget.position - transform.position;
            Character.Move(direction);
        }
        else Character.Idle();

    }

    private void Collider2DTrigger()
    {
        var colider = Physics2D.OverlapCircle(transform.position, _rangerDetect, TargetLayer);

        if (colider) Target = colider.transform;
        else Target = null;
    }

    public void Init(LayerMask layer, Transform defaultTarget = null)
    {
        TargetLayer = layer;
        DefaultTarget = defaultTarget;
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

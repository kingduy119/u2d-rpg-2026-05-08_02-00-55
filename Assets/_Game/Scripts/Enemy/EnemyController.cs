using UnityEngine;
[RequireComponent(typeof(EnemyMovement))]

[RequireComponent(typeof(EnemyCombat))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    public Transform Target { get; set; }
    public Vector2 Direction { get; private set; }
    private EnemyStateMachine m_stateMachine;

    public EnemyStateMachine StateMachine => m_stateMachine;
    public Animator Animator => m_animator;
    public EnemyMovement Movement { get; private set; }
    public EnemyCombat Combat { get; private set; }

    private void Awake()
    {
        m_stateMachine = new(this);

        Movement = GetComponent<EnemyMovement>();
        Combat = GetComponent<EnemyCombat>();

        Movement.Initialize(this);
        Combat.Initialize(this);
    }

    private void Start()
    {
        m_stateMachine.Initialize(m_stateMachine.m_idleState);
    }

    private void Update()
    {
        m_stateMachine.Execute();
        Combat.CheckPlayerInAttackRange();

        CheckTargetInRange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Target = null;
        }
    }

    private void CheckTargetInRange()
    {
        if (Target == null)
            return;

        Direction = (Target.position - transform.position).normalized;
        if (Direction.x > 0 && transform.localScale.x < 0 ||
       Direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

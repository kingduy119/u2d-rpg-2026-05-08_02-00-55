using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyController : MonoBehaviour
{
    private EnemyStateMachine m_stateMachine;
    // private Animator _anim;

    public string StateString { get; set; }
    public EnemyMovement Movement { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<EnemyMovement>();

        m_stateMachine = new(this);
        Movement.Initialize(this);
    }

    public EnemyStateMachine StateMachine => m_stateMachine;

    private void Start()
    {
        m_stateMachine.Initialize(m_stateMachine.m_idleState);
    }

    private void Update()
    {
        m_stateMachine.Execute();
    }
}

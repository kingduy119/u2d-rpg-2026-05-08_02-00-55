using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform m_detectionPoint;
    [SerializeField] private LayerMask m_playerLayer;

    private Rigidbody2D m_rigid;

    public Transform m_target;
    public float m_maxSpeed;
    public float m_moveSpeed;

    private EnemyController m_controller;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_target = collision.transform;
            m_controller.StateMachine.TransitionTo(m_controller.StateMachine.m_chaseState);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        m_controller.StateMachine.TransitionTo(m_controller.StateMachine.m_idleState);
    }

    public void Initialize(EnemyController controller)
    {
        m_controller = controller;
    }


    public void Chase()
    {
        m_moveSpeed = Mathf.Clamp(m_moveSpeed, 0, m_maxSpeed);
        Vector2 direction = (m_target.position - transform.position).normalized;
        m_rigid.linearVelocity = direction * m_moveSpeed;

        if (direction.x > 0 && transform.localScale.x < 0 ||
           direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    public void Stop()
    {
        m_target = null;
        m_rigid.linearVelocity = Vector2.zero;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

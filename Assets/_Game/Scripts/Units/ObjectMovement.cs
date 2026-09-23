using UnityEngine;
using UnityEngine.InputSystem;

public enum StateEnum
{
    Idle,
    Moving,
    Chasing,
    Attacking,
    KnockBack,
    Dead
}

public class ObjectMovement : MonoBehaviour
{
    protected StateEnum state = StateEnum.Idle;
    public float m_MoveSpeed = 2f;
    public float m_MaxSpeed = 2f;
    private Vector2 m_MoveDirection = Vector2.zero;
    public InputAction m_MoveAction;

    protected Rigidbody2D rb;
    protected Animator anim;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void HandleInput()
    {
        m_MoveDirection = m_MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(m_MoveDirection.x, 0.0f) || !Mathf.Approximately(m_MoveDirection.y, 0.0f))
        {
            m_MoveDirection.Set(m_MoveDirection.x, m_MoveDirection.y);
            m_MoveDirection.Normalize();
        }
        else
        {
            m_MoveDirection.Set(0, 0);
        }

        anim.SetFloat("moveX", m_MoveDirection.x);
        anim.SetFloat("moveY", m_MoveDirection.y);
        anim.SetFloat("speed", m_MoveDirection.magnitude);

        if (m_MoveDirection.x > 0 && transform.localScale.x < 0 ||
            m_MoveDirection.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    protected virtual void Move()
    {
        rb.linearVelocity = m_MoveDirection * m_MoveSpeed;
    }

    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ChangeState(StateEnum newState)
    {
        if (state == StateEnum.Moving || state == StateEnum.Chasing)
            anim.SetBool("isMoving", false);

        state = newState;
        if (state == StateEnum.Moving || state == StateEnum.Chasing)
            anim.SetBool("isMoving", true);
        else if (state == StateEnum.Attacking)
            anim.SetTrigger("isAttack1");
    }

}


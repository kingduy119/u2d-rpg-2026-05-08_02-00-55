using System.Collections;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    public State state = State.Idle;

    public float m_MoveSpeed = 2f;
    public float m_MaxSpeed = 2f;
    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 m_Direction;
    public bool isPlayerInput = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (isPlayerInput)
        {
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController == null)
            {
                playerController = gameObject.AddComponent<PlayerController>();
            }
            playerController.enabled = true;
        }
    }


    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                Stop();
                break;
            case State.Moving:
                Move();
                break;
            case State.Chasing:
                // Handle chasing behavior
                break;
            case State.Attacking:
                // Handle attacking behavior
                break;
            case State.KnockBack:
                // Handle knockback behavior
                break;
            case State.Dead:
                // Handle death behavior
                break;
        }
    }

    public void SetDirection(Vector2 direction)
    {
        m_Direction = direction.normalized;
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    void Move()
    {
        // Vector2 direction = ((Vector3)target - transform.position).normalized;
        rb.linearVelocity = m_Direction * m_MoveSpeed;

        anim.SetFloat("aimX", Mathf.Abs(m_Direction.x));
        anim.SetFloat("aimY", Mathf.Abs(m_Direction.y));
        anim.SetFloat("speed", m_Direction.magnitude);

        if (m_Direction.x > 0 && transform.localScale.x < 0 ||
           m_Direction.x < 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetFloat("speed", 0);
    }
}

using UnityEngine;
using System;

public class ObjectMovement : MonoBehaviour
{
    protected State state = State.Idle;
    public float moveSpeed = 2f;
    protected float moveX;
    protected float moveY;

    protected Rigidbody2D rb;
    protected Animator anim;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void HandleMovement()
    {
        if (moveX > 0 && transform.localScale.x < 0 ||
            moveX < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected virtual void ChangeState(State newState)
    {
        if (state == State.Moving || state == State.Chasing)
            anim.SetBool("isMoving", false);

        state = newState;
        if (state == State.Moving || state == State.Chasing)
            anim.SetBool("isMoving", true);
        else if (state == State.Attacking)
            anim.SetTrigger("isAttack1");
    }
}

public enum State
{
    Idle,
    Moving,
    Chasing,
    Attacking,
    Dead
}
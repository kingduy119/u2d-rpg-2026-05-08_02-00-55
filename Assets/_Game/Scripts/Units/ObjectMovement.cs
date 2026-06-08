using UnityEngine;
using System;

public enum State
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
    protected State state = State.Idle;
    public float moveSpeed = 2f;
    public float maxSpeed = 2f;


    protected Rigidbody2D rb;
    protected Animator anim;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ChangeState(State newState)
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


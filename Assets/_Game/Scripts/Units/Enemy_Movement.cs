using UnityEngine;

public class Enemy_Movement : ObjectMovement
{
    private Transform target;

    public float detectionRange = 2f;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    public float attackRange = 1.2f;
    public float attackSpeed = 5f;
    public float attackCountDown = 0f;

    public bool isKnockedBack = false;


    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (state == State.KnockBack)
            return;

        CheckForPlayer();

        if (state == State.Chasing)
        {
            Chase();
        }
        else if (state == State.Attacking)
        {
            rb.linearVelocity = Vector2.zero;

            if (attackCountDown <= 0)
            {
                Attack();
            }
        }
        else
        {
            target = null;
            rb.linearVelocity = Vector2.zero;
        }

        if (attackCountDown > 0)
        {
            attackCountDown -= Time.deltaTime;
        }
    }

    void CheckForPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPoint.position, detectionRange, playerLayer);
        if (colliders.Length > 0)
        {
            target = colliders[0].transform;
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance <= attackRange && attackCountDown <= 0)
                ChangeState(State.Attacking);
            else if (distance > attackRange)
            {
                ChangeState(State.Chasing);
            }
            else
            {
                ChangeState(State.Idle);
            }
        }
        else
        {
            ChangeState(State.Idle);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeState(State.Idle);
        }
    }

    void Chase()
    {
        moveSpeed = maxSpeed;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0 && transform.localScale.x < 0 ||
           direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    void Attack()
    {
        moveSpeed = 0f;
        attackCountDown = attackSpeed;
        rb.linearVelocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectionPoint.position, detectionRange);

        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
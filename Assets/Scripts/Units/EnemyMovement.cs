using UnityEngine;

public class EnemyMovement : ObjectMovement
{
    private bool isChasing = false;
    private Transform target;

    public float detectionRange = 2f;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    public float attackRange = 1.2f;
    public float attackSpeed = 5f;
    private float attackCountdown = 0f;

    public bool isKnockedBack = false;


    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        CheckForPlayer();

        if (state == State.Chasing)
        {
            Chase();
        }
        else if (state == State.Attacking)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (attackCountdown > 0)
        {
            attackCountdown -= Time.deltaTime;
        }
    }

    void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, detectionRange, playerLayer);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
            if (Vector2.Distance(transform.position, target.position) <= attackRange && attackCountdown <= 0)
                ChangeState(State.Attacking);
            else if (Vector2.Distance(transform.position, target.position) > attackRange && state != State.Attacking)
            {
                rb.linearVelocity = Vector2.zero;
                ChangeState(State.Chasing);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                ChangeState(State.Idle);
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     // if (collision.gameObject.CompareTag("Player"))
    //     // {
    //     //     target = collision.transform;
    //     //     // ChangeState(State.Chasing);
    //     //     if (Vector2.Distance(transform.position, target.position) <= attackRange && attackCountdown <= 0)
    //     //         ChangeState(State.Attacking);
    //     //     else if (Vector2.Distance(transform.position, target.position) > attackRange)
    //     //     {
    //     //         rb.linearVelocity = Vector2.zero;
    //     //         ChangeState(State.Chasing);
    //     //     }
    //     //     else
    //     //     {
    //     //         rb.linearVelocity = Vector2.zero;
    //     //         ChangeState(State.Idle);
    //     //     }
    //     // }
    // }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // target = null;
            ChangeState(State.Idle);
        }
    }

    void Chase()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0 && transform.localScale.x < 0 ||
           direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }


    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectionPoint.position, detectionRange);
    }

}
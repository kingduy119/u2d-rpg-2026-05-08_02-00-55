using System.Collections;
using UnityEngine;

public class NPC_Wander : MonoBehaviour
{
    [Header("Wander Area")]
    public float width = 5f;
    public float height = 5f;
    public Vector2 startPosition;

    public float speed = 2f;
    private Vector2 target;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isPaused;
    public float pauseDuration = 1.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        target = GetRandomTarget();
    }

    private void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance(transform.position, target) < .1f)
            StartCoroutine(PauseAndPickNewDestination());

        Move();
    }

    void Move()
    {
        Vector2 direction = ((Vector3)target - transform.position).normalized;
        rb.linearVelocity = direction * speed;
        if (direction.x > 0 && transform.localScale.x < 0 ||
           direction.x < 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        rb.linearVelocity = Vector2.zero;
        anim.Play("pawn_idle");

        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();
        isPaused = false;
        anim.Play("pawn_run");
    }

    private Vector2 GetRandomTarget()
    {
        float randomX = Random.Range(startPosition.x - width / 2, startPosition.x + width / 2);
        float randomY = Random.Range(startPosition.y - height / 2, startPosition.y + height / 2);
        return new Vector2(randomX, randomY);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startPosition, new Vector3(width, height, 0));
    }
}

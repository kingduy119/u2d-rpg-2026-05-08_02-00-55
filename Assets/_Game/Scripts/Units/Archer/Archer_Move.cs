using System.Collections;
using UnityEngine;

public class Archer_Move : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxSpeed = 2f;
    public Vector2 zoneSize = new Vector2(5f, 5f);
    public Rigidbody2D rb;
    public Animator anim;

    private Vector2 target;
    public Vector2 startPosition;
    private bool isPaused;
    public float pauseDuration = 1.5f;

    void Start()
    {
        target = GetRandomTarget();
        Debug.Log($"Start Position: {startPosition} | Initial Target: {target}");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return;

        if (Vector2.Distance(transform.position, target) < .1f)
            StartCoroutine(PauseAndPickNewDestination());

        Move();
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetFloat("moveX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("moveY", Mathf.Abs(rb.linearVelocity.y));

        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();
        isPaused = false;
    }

    void Move()
    {
        Vector2 direction = ((Vector3)target - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        if (direction.x > 0 && transform.localScale.x < 0 ||
           direction.x < 0 && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        anim.SetFloat("moveX", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("moveY", Mathf.Abs(rb.linearVelocity.y));
        Debug.Log($"Current Velocity: {rb.linearVelocity}, Target: {target}");
    }

    private Vector2 GetRandomTarget()
    {
        float randomX = Random.Range(startPosition.x - zoneSize.x / 2, startPosition.x + zoneSize.x / 2);
        float randomY = Random.Range(startPosition.y - zoneSize.y / 2, startPosition.y + zoneSize.y / 2);
        return new Vector2(randomX, randomY);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector3)startPosition, new Vector3(zoneSize.x, zoneSize.y, 0));
    }
}

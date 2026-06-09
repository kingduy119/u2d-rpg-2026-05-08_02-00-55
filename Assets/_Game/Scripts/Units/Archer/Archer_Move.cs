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
        Debug.Log($"New Target Start: {target}");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            Stop();
            return;
        }

        if (Vector2.Distance(transform.position, target) < .1f)
            StartCoroutine(PauseAndPickNewDestination());

        Move();
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();
        isPaused = false;
    }

    void Move()
    {
        Vector2 direction = ((Vector3)target - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        anim.SetFloat("moveX", Mathf.Abs(direction.x));
        anim.SetFloat("moveY", Mathf.Abs(direction.y));
        anim.SetFloat("speed", direction.magnitude);

        if (direction.x > 0 && transform.localScale.x < 0 ||
           direction.x < 0 && transform.localScale.x > 0)
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

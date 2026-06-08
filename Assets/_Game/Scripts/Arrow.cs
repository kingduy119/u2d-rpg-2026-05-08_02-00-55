using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public float lifeSpawn = 2;
    public float speed;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        Destroy(gameObject, lifeSpawn);
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Arrow OnCollisionEnter2D");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy_Health>().ChangeHealth(-StateManager.Instance.damage);
            collision.gameObject.GetComponent<Enemy_Knockback>().KnockBack(
                transform,
                StateManager.Instance.knockbackForce,
                StateManager.Instance.stunTime
            );
            AttachToTarget(collision.gameObject.transform);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle");
            AttachToTarget(collision.gameObject.transform);
        }
    }

    private void AttachToTarget(Transform target)
    {
        Debug.Log("AttachToTarget");
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        // rb.bodyType = RigidbodyType2D.Dynamic;

        transform.SetParent(target);
    }
}

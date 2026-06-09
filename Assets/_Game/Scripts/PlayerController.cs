using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public InputAction MoveAction;

    private Vector2 move;
    private Vector2 moveDirection;
    public float moveSpeed = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MoveAction.Enable();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Vector2 position = rb.position + moveSpeed * Time.fixedDeltaTime * move;
        rb.MovePosition(position);
    }

    void HandleInput()
    {
        move = MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        else
        {
            moveDirection.Set(0, 0);
        }

        animator.SetFloat("moveX", moveDirection.x);
        animator.SetFloat("moveY", moveDirection.y);
        animator.SetFloat("speed", moveDirection.magnitude);

        if (moveDirection.x > 0 && transform.localScale.x < 0 ||
            moveDirection.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

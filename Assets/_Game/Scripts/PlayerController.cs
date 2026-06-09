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
        // Arrow keys or WASD input
        move = MoveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        animator.SetFloat("moveX", moveDirection.x);
        animator.SetFloat("moveY", moveDirection.y);
    }
}

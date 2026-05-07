using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Animator anim;

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    // }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (moveX > 0 && transform.localScale.x < 0 ||
            moveX < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.SetFloat("moveX", Mathf.Abs(moveX));
        anim.SetFloat("moveY", Mathf.Abs(moveY));

        rb.linearVelocity = new Vector2(moveX, moveY) * speed;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

using System;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // public float speed = 5f;
    public Rigidbody2D rb;
    public Animator anim;
    public Joystick joystick;

    public bool isKnockedBack = false;

    float moveX;
    float moveY;

    float joystickX = 0f;
    float joystickY = 0f;

    public Player_Combat player_Combat;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            player_Combat.Attack();
        }
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
            return;

        float keyboardX = Input.GetAxis("Horizontal");
        float keyboardY = Input.GetAxis("Vertical");

        if (joystick != null)
        {
            joystickX = joystick.Horizontal;
            joystickY = joystick.Vertical;
        }


        moveX = Mathf.Abs(joystickX) > 0.1f ? joystickX : keyboardX;
        moveY = Mathf.Abs(joystickY) > 0.1f ? joystickY : keyboardY;

        if (moveX > 0 && transform.localScale.x < 0 ||
            moveX < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.SetFloat("moveX", Mathf.Abs(moveX));
        anim.SetFloat("moveY", Mathf.Abs(moveY));

        rb.linearVelocity = new Vector2(moveX, moveY) * StateManager.Instance.speed;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void KnockBack(Transform enemy, float force, float duration = 0.5f)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * force;
        StartCoroutine(KnockBackCoroutine(duration));
    }

    IEnumerator KnockBackCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
}

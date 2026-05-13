using UnityEngine;
using System.Collections;

public class Enemy_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isKnockedBack = false;
    private Enemy_Movement enemy_Movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy_Movement = GetComponent<Enemy_Movement>();
    }

    public void KnockBack(Transform player, float force, float duration = 0.5f)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - player.position).normalized;
        rb.linearVelocity = direction * force;
        enemy_Movement.ChangeState(State.KnockBack);
        StartCoroutine(KnockBackCoroutine(duration));
    }

    IEnumerator KnockBackCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
        enemy_Movement.ChangeState(State.Idle);
    }
}

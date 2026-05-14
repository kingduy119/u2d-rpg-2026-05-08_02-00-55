using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    public Animator animator;
    private float attackCountDown = 0f;

    public Transform attackPoint;
    public LayerMask enemyLayer;


    void Update()
    {
        if (attackCountDown > 0)
        {
            attackCountDown -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (attackCountDown <= 0)
        {
            animator.SetBool("isAttacking1", true);
            attackCountDown = StateManager.Instance.attackSpeed;
        }
    }

    public void Attack_Done()
    {
        animator.SetBool("isAttacking1", false);
    }

    public void Deal_Damge()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            StateManager.Instance.weaponRange,
            enemyLayer
        );
        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<Enemy_Knockback>().KnockBack(
                transform,
                StateManager.Instance.knockbackForce,
                StateManager.Instance.stunTime
            );
            enemies[0].GetComponent<Enemy_Health>().ChangeHealth(-StateManager.Instance.damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, StateManager.Instance.weaponRange);
    }
}

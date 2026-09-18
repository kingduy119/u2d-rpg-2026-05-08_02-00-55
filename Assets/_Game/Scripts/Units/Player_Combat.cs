using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    private Animator animator;
    private float attackCountDown = 0f;
    public float weaponRange = 0.5f;

    public LayerMask enemyLayer;
    public Transform attackPoint;
    public Transform shootPoint;

    public bool m_ShowDrawGizmo = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        if (attackPoint == null || !m_ShowDrawGizmo)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}

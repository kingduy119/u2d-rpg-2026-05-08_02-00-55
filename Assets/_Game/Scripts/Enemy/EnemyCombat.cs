using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    public float m_attackRange = 0.2f;
    public float m_detectRange = 0.2f;
    public Transform m_detectionPoint;
    public Transform m_attackPoint;
    public LayerMask m_playerLayer;

    public Transform m_target;
    public float m_damage = 0.2f;
    public float m_attackSpeed = 2f;
    public float m_attackCooldown = 0f;
    public float m_knockForce = 1f;
    public float m_stunTime = 1f;
    public bool IsKnockedBack = false;
    public bool IsAttacking = false;
    public bool IsPlayerInAttackRange = false;

    public bool CanAttack => m_attackCooldown <= 0f && !IsAttacking && IsPlayerInAttackRange;

    private EnemyController m_Enemy;

    public void Initialize(EnemyController controller)
    {
        m_Enemy = controller;
    }

    private void Update()
    {
        if (IsKnockedBack)
            return;

        if (m_attackCooldown > 0)
        {
            m_attackCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Enemy.StateMachine.TransitionTo(m_Enemy.StateMachine.m_idleState);
    }

    public void CheckPlayerInAttackRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_detectionPoint.position, m_detectRange, m_playerLayer);
        if (colliders.Length > 0)
        {
            m_target = colliders[0].transform;
            float distance = Vector2.Distance(transform.position, m_target.position);
            IsPlayerInAttackRange = distance <= m_detectRange;
        }
        else
        {
            IsPlayerInAttackRange = false;
        }
    }

    public void LaunchAttack()
    {
        IsAttacking = true;
        m_Enemy.Animator.SetTrigger("isAttack1");
    }

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRange, m_playerLayer);
        foreach (Collider2D player in hits)
        {
            StateManager.Instance.UpdateHealth(-m_damage);
            player.GetComponent<Player>().KnockBack(transform, m_knockForce, m_stunTime);
        }
        m_attackCooldown = m_attackSpeed;
        IsAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_detectionPoint.position, m_detectRange);

        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(m_attackPoint.position, m_attackRange);
    }
}



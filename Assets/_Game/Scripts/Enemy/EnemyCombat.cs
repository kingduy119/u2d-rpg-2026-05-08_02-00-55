using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    public float m_range = 0.2f;
    public Transform m_detectionPoint;
    public LayerMask m_playerLayer;

    private Transform m_target;
    public float m_damage = 0.2f;
    public float m_attackSpeed = 5f;
    public float attackCountDown = 0f;

    public bool isKnockedBack = false;

    private EnemyController m_controller;

    private void Update()
    {
        CheckForPlayer();
    }

    public void Initialize(EnemyController controller)
    {
        m_controller = controller;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        m_controller.StateMachine.TransitionTo(m_controller.StateMachine.m_idleState);
    }

    private void CheckForPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_detectionPoint.position, m_range, m_playerLayer);
        if (colliders.Length > 0)
        {

            m_target = colliders[0].transform;
            float distance = Vector2.Distance(transform.position, m_target.position);
            if (distance <= m_range)
            {
                m_controller.StateMachine.TransitionTo(m_controller.StateMachine.m_attackState);
            }
            else
            {
                m_controller.StateMachine.TransitionTo(m_controller.StateMachine.m_chaseState);
            }
        }
    }
}



using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D m_rigid;

    public float m_maxSpeed;
    public float m_moveSpeed;

    private EnemyController Enemy;
    public Transform m_target;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
    }

    public void Initialize(EnemyController enemy)
    {
        Enemy = enemy;
    }

    public void Chase()
    {
        if (Enemy.Target == null)
            return;

        m_moveSpeed = Mathf.Clamp(m_moveSpeed, 0, m_maxSpeed);
        m_rigid.linearVelocity = Enemy.Direction * m_moveSpeed;
        Enemy.Animator.SetFloat("Speed", m_rigid.linearVelocity.magnitude);
    }

    public void Stop()
    {
        m_rigid.linearVelocity = Vector2.zero;
        Enemy.Animator.SetFloat("Speed", m_rigid.linearVelocity.magnitude);
    }
}

using UnityEngine;

public class IdleState : IState
{
    private EnemyController m_enemy;

    public IdleState(EnemyController enemy)
    {
        m_enemy = enemy;
    }

    public void Execute()
    {
        if (m_enemy.Combat.CanAttack)
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_combatState);
        }
        else if (m_enemy.Target && !m_enemy.Combat.IsPlayerInAttackRange)
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_chaseState);
        }
    }
}

public class ChaseState : IState
{
    private EnemyController m_enemy;
    public ChaseState(EnemyController enemy)
    {
        m_enemy = enemy;
    }

    public void Execute()
    {
        if (m_enemy.Target == null || m_enemy.Target && m_enemy.Combat.IsPlayerInAttackRange)
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_idleState);
            return;
        }

        if (m_enemy.Combat.CanAttack)
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_combatState);
            return;
        }

        m_enemy.Movement.Chase();
    }

    public void Exit()
    {
        m_enemy.Movement.Stop();
    }
}

public class CombatState : IState
{
    private EnemyController m_enemy;
    public CombatState(EnemyController enemy)
    {
        m_enemy = enemy;
    }

    public void Enter()
    {
        // m_enemy.Movement.Stop();
    }

    public void Execute()
    {
        if (!m_enemy.Combat.IsPlayerInAttackRange)
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_chaseState);
        }
        else if (m_enemy.Combat.CanAttack)
        {
            m_enemy.Combat.LaunchAttack();
        }
        else
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_idleState);
        }
    }
}

public class IdleState : IState
{
    private EnemyController m_enemy;

    public IdleState(EnemyController enemy)
    {
        m_enemy = enemy;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        if (m_enemy.StateMachine.CurrentState is ChaseState)
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_chaseState);
        }
        else
        {
            m_enemy.Movement.Stop();
        }
    }

    public void Exit()
    {
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
        if (m_enemy.StateMachine.CurrentState is IdleState)
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_idleState);
        }
        else
        {
            m_enemy.Movement.Chase();
        }
    }
}

public class CombatState : IState
{
    private EnemyController m_enemy;
    public CombatState(EnemyController enemy)
    {
        m_enemy = enemy;
    }

    public void Execute()
    {
        if (m_enemy.StateMachine.CurrentState is IdleState)
        {
            m_enemy.StateMachine.TransitionTo(m_enemy.StateMachine.m_idleState);
        }
        else
        {
            m_enemy.Movement.Chase();
        }
    }
}
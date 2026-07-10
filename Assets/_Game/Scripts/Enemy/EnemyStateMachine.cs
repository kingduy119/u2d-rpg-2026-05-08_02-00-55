using System;

public class EnemyStateMachine
{
    public IState CurrentState { get; private set; }
    public IdleState m_idleState;
    public ChaseState m_chaseState;
    public CombatState m_attackState;

    public event Action<IState> stateChanged;

    public EnemyStateMachine(EnemyController enemy)
    {
        m_idleState = new(enemy);
        m_chaseState = new(enemy);
        m_attackState = new(enemy);
    }

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();
        stateChanged?.Invoke(state);
    }

    public void TransitionTo(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();

        stateChanged?.Invoke(newState);
    }

    public void Execute()
    {
        if (CurrentState != null)
            CurrentState.Execute();
    }
}

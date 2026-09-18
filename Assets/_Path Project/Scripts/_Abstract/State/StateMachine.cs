
public class StateMachine
{
    public IState CurrentState { get; set; }
    // public event Action<IState> stateChanged;

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();

        // notify other objects that state has changed
        // stateChanged?.Invoke(state);
    }

    public void TransitionTo(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        newState.Enter();
    }

    public void Execute()
    {
        CurrentState?.Execute();
    }
}





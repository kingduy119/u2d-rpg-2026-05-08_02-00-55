
namespace TDGame
{
    public class StateMachine
    {
        public IState CurrentState { get; set; }

        public void Execute()
        {
            CurrentState?.Execute();
        }

        public void TransitionTo(IState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            newState.Enter();
        }
    }


}
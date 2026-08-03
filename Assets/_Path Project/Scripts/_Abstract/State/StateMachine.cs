
namespace TDGame
{
    public class StateMachine
    {
        public IState CurrentState { get; set; }

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


}
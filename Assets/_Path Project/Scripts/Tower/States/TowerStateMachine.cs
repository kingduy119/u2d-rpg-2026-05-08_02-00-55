
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


    public class TowerStateMachine : StateMachine
    {
        public TowerIdleState TowerIdleState { get; private set; }
        public TowerBuildedState TowerBuildedState { get; private set; }

        public TowerStateMachine(Tower tower)
        {
            TowerIdleState = new TowerIdleState(tower);
            TowerBuildedState = new TowerBuildedState(tower);

            CurrentState = TowerBuildedState;
        }
    }
}
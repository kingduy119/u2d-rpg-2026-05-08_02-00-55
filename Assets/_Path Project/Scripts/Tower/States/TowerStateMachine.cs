
namespace TDGame
{
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
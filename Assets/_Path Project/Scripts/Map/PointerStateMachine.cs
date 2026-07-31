namespace TDGame
{

    public class PointerStateMachine
    {
        public IState CurrentState { get; private set; }
        public PointerHoverState PointerHoverState;
        public PointerBuildTowerState PointerBuildTowerState;

        public PointerStateMachine(WorldMap worldmap)
        {
            PointerHoverState = new(worldmap);
            PointerBuildTowerState = new(worldmap);

            CurrentState = PointerHoverState;
        }

        public void Enable()
        {
            TowerEvent.OnTowerCardSelect += TowerCardSelect;
        }

        public void Disable()
        {
            TowerEvent.OnTowerCardSelect -= TowerCardSelect;
        }

        public void Execute()
        {
            CurrentState?.Execute();
        }

        public void TransitionTo(IState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            newState.Enter();
        }

        private void TowerCardSelect(TowerSO towerSO)
        {
            TransitionTo(PointerBuildTowerState);
            PointerBuildTowerState.HandleTowerCardSelect(towerSO);
        }
    }



    // End
}
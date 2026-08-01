
namespace TDGame
{

    public class TowerIdleState : IState
    {
        private readonly Tower Tower;

        public TowerIdleState(Tower tower)
        {
            Tower = tower;
        }

        public void Enter()
        {
            Tower.Combat.enabled = false;
            Tower.Hover.enabled = false;
        }
    }


    public class TowerBuildedState : IState
    {
        private readonly Tower Tower;

        public TowerBuildedState(Tower tower)
        {
            Tower = tower;
        }

        public void Enter()
        {
            Tower.Combat.enabled = true;
            Tower.Hover.enabled = true;
        }
    }

}
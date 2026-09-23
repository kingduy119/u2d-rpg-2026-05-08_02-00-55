
namespace TDGame
{

    public class TowerIdleState : State
    {
        private readonly Tower Tower;

        public TowerIdleState(Tower tower)
        {
            Tower = tower;
        }

        public override void Enter()
        {
            Tower.Hover.enabled = false;
            Tower.Combat.enabled = false;
            Tower.Combat.ShootRanageArea.enabled = true;
        }
    }


    public class TowerBuildedState : State
    {
        private readonly Tower Tower;

        public TowerBuildedState(Tower tower)
        {
            Tower = tower;
        }

        public override void Enter()
        {
            Tower.Combat.enabled = true;
            Tower.Hover.enabled = true;
            Tower.Combat.ShootRanageArea.enabled = false;
        }
    }

}


namespace TDGame
{

    public class TowerUpdateButton : ButtonBase
    {

        protected override void HandleClick()
        {
            TowerEvent.OnSelectUpdateTower?.Invoke();
        }
    }
}

namespace TDGame
{

    public class TowerBuildButton : ButtonBase
    {
        protected override void HandleClick()
        {
            TowerEvent.AcceptBuild?.Invoke();
        }

    }
}

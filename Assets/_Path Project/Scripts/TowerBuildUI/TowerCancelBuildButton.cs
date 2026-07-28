namespace TDGame
{

    public class TowerCancelBuildButton : ButtonBase
    {
        protected override void HandleClick()
        {
            TowerEvent.OnCancelBuild?.Invoke();
        }

    }
}


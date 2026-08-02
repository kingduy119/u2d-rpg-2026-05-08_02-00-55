
namespace TDGame
{
    public class PauseGameButtonUI : ButtonBase
    {

        protected override void HandleClick()
        {
            InGameEvent.OnPauseGame?.Invoke();
        }
    }
}


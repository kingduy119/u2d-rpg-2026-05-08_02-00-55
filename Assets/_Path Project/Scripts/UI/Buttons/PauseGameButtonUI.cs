
namespace TDGame
{
    public class PauseGameButtonUI : ButtonBase
    {

        protected override void HandleClick()
        {
            GamePlayEvent.OnPauseGame?.Invoke();
        }
    }
}


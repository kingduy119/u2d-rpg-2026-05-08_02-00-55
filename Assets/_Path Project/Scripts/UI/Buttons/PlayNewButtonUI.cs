


namespace TDGame
{
    public class PlayNewButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            GameEvent.OnPlayNewGame?.Invoke(0);
        }
    }
}
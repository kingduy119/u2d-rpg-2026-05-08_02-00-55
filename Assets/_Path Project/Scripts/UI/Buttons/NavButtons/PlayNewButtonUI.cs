




namespace TDGame
{
    public class PlayNewButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            GameEvent.PlayNewGame?.Invoke(0);
        }
    }
}
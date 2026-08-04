


namespace TDGame
{
    public class PlayContinueButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            GameEvent.PlayContinue?.Invoke();
        }
    }
}
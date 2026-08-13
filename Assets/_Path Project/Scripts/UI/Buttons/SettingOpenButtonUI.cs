


namespace TDGame
{
    public class SettingOpenButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            // GameEvent.OnPlayNewGame?.Invoke(0);
            GamePlayEvent.SettingClick?.Invoke();
        }
    }
}
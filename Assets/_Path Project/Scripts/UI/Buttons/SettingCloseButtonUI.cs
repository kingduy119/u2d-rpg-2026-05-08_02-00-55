

namespace TDGame
{
    public class SettingCloseButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            GameEvent.SettingClose?.Invoke();
        }
    }
}



// using UnityEngine.SceneManagement;

namespace TDGame
{
    public class MainMenuButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            GamePlayEvent.MainMenuClick?.Invoke();
        }
    }
}
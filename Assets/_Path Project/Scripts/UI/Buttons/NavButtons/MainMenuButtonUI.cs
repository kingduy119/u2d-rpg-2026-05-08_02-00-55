


// using UnityEngine.SceneManagement;

namespace TDGame
{
    public class MainMenuButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            // GameEvent.LoadScene("TD_MainMenu");
            GamePlayEvent.MainMenuClick?.Invoke();
        }
    }
}
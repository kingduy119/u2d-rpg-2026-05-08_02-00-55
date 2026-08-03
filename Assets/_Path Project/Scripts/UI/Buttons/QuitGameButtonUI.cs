
namespace TDGame
{
    public class QuitGameButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

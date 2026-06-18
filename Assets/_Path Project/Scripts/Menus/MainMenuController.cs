using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("TD_Gameplay");
    }

    public void RestartGame()
    {
        // TDGameManager.Instance.RestartLevel();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

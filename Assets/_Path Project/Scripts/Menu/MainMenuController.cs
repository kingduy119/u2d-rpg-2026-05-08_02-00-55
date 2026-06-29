using TMPro;
using UnityEngine;

namespace TDGame
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtLevel;

        private void OnEnable()
        {
            GameEvent.OnUpdateUI += UpdateUI;
        }

        private void OnDisable()
        {
            GameEvent.OnUpdateUI -= UpdateUI;
        }

        private void UpdateUI()
        {
            txtLevel.text = LevelManager.Instance.LevelSO.levelName;
        }

        public void StartNewGame()
        {
            LevelManager.Instance.LoadLevel(0);
        }

        public void PlayContinue()
        {
            LevelManager.Instance.PlayContinue();
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
}
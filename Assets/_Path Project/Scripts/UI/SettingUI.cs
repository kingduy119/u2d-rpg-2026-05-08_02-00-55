using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TDGame
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private Button _close;
        [SerializeField] private Button _resetLevel;
        [SerializeField] private Button _mainMenu;
        [SerializeField] private Button _audios;
        [SerializeField] private Button _audiosBack;

        [SerializeField] private GameObject _buttonsPanel;
        [SerializeField] private GameObject _audiosPanel;

        private void OnEnable()
        {
            _close.onClick.AddListener(CloseSettingClick);
            _resetLevel.onClick.AddListener(ResetLevelClick);
            _mainMenu.onClick.AddListener(MainMenuClick);
            _audios.onClick.AddListener(DisplayAudio);
            _audiosBack.onClick.AddListener(HiddenAudio);
        }

        private void OnDisable()
        {
            _close.onClick.RemoveListener(CloseSettingClick);
            _resetLevel.onClick.RemoveListener(ResetLevelClick);
            _mainMenu.onClick.RemoveListener(MainMenuClick);
            _audios.onClick.RemoveListener(DisplayAudio);
            _audiosBack.onClick.RemoveListener(HiddenAudio);
        }

        private void CloseSettingClick()
        {
            GameEvent.ResumeGame();
            gameObject.SetActive(false);
        }

        private void ResetLevelClick()
        {
            GameEvent.SendRestartGame();
        }

        private void MainMenuClick()
        {
            SceneManager.LoadScene("TD_MainMenu");
        }

        private void DisplayAudio()
        {
            _buttonsPanel.SetActive(false);
            _audiosPanel.SetActive(true);
        }

        private void HiddenAudio()
        {
            _buttonsPanel.SetActive(true);
            _audiosPanel.SetActive(false);
        }
    }
}
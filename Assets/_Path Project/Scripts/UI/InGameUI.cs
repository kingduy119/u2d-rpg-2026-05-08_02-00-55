using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public class InGameUI : MonoBehaviour
    {
        [Header("UI Text")]
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text rockText;
        [SerializeField] private TMP_Text woodText;
        [SerializeField] private TMP_Text alertText;
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private TMP_Text enemiesText;

        [Header("UI Buttons")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button startWaveButton;
        [SerializeField] private Button gameSpeedButton;
        private TMP_Text gameSpeedText;

        [Header("UI Pannels")]
        [SerializeField] private GameObject settingsPanel;

        private GameState GameState;

        private void Awake()
        {
            settingsPanel.SetActive(false);
            gameSpeedText = gameSpeedButton.GetComponentInChildren<TMP_Text>();

            GameState = GameManager.Instance.GameState;
        }

        private void OnEnable()
        {
            settingsButton.onClick.AddListener(HandleSettingsClick);
            startWaveButton.onClick.AddListener(GameState.StartWave);
            gameSpeedButton.onClick.AddListener(GameState.ChangeGameSpeed);

            GameEvent.OnLoadLevel += LoadLevelResource;
        }

        private void OnDisable()
        {
            settingsButton.onClick.RemoveListener(HandleSettingsClick);
            startWaveButton.onClick.RemoveListener(GameState.StartWave);
            gameSpeedButton.onClick.RemoveListener(GameState.ChangeGameSpeed);

            GameEvent.OnLoadLevel -= LoadLevelResource;
        }

        private void Update()
        {
            if (GameState.IsDirty)
            {
                UpdateInGameUI();
                GameState.Clearn();
            }

        }

        private void UpdateInGameUI()
        {
            goldText.SetText("{0}", GameState.Golds);
            rockText.SetText("{0}", GameState.Rocks);
            woodText.SetText("{0}", GameState.Woods);
            livesText.SetText("{0}", GameState.Lives);
            waveText.SetText("{0}", GameState.WaveCount + 1);
            enemiesText.SetText("{0}", GameState.Enemies);

            gameSpeedText.SetText($"x{GameState.GameSpeed}");
            startWaveButton.interactable = !GameState.IsStarted;
        }

        private void LoadLevelResource()
        {
            LevelSO level = GameManager.Instance.LevelManager.LevelState.CurrentLevel;
            GameState.Golds = level.startingGold;
            GameState.Lives = level.startingLives;
        }

        public void OnCloseSettingsClick() => ResumeGame();
        private void HandleSettingsClick()
        {
            GameEvent.PauseGame();
            settingsPanel.SetActive(true);
        }
        public void PauseGame() => GameEvent.PauseGame();
        public void ResumeGame() => GameEvent.ResumeGame();
    }
}
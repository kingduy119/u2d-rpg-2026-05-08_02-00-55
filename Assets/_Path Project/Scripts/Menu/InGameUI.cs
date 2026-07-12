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

        private InGameState InGameState => GameManager.Instance.InGameState;

        private void Awake()
        {
            settingsPanel.SetActive(false);
            gameSpeedText = gameSpeedButton.GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            if (InGameState != null)
            {
                GameEvent.OnEnemyReachedEnd += InGameState.HandlePointReachedEnd;
                GameEvent.OnGetEnemyReward += InGameState.HandleGetEnemyReward;
                SpawnManager.OnWaveChanged += InGameState.HandleWaveChanged;
            }

            settingsButton.onClick.AddListener(HandleSettingsClick);
            startWaveButton.onClick.AddListener(HandleStartWaveClick);
            gameSpeedButton.onClick.AddListener(HandleGameSpeedClick);

            GameEvent.OnLoadLevel += LoadLevelResource;
        }

        private void OnDisable()
        {
            if (InGameState != null)
            {
                GameEvent.OnEnemyReachedEnd -= InGameState.HandlePointReachedEnd;
                GameEvent.OnGetEnemyReward -= InGameState.HandleGetEnemyReward;
                SpawnManager.OnWaveChanged -= InGameState.HandleWaveChanged;
            }


            settingsButton.onClick.RemoveListener(HandleSettingsClick);
            startWaveButton.onClick.RemoveListener(HandleStartWaveClick);
            gameSpeedButton.onClick.RemoveListener(HandleGameSpeedClick);

            GameEvent.OnLoadLevel -= LoadLevelResource;
        }

        private void Update()
        {
            if (InGameState.IsDirty)
            {
                UpdateInGameUI();
                InGameState.Clearn();
            }

        }

        private void LoadLevelResource()
        {
            LevelSO level = LevelManager.Instance.LevelSO;
            InGameState.Golds = level.startingGold;
            InGameState.Lives = level.startingLives;
        }

        private void UpdateInGameUI()
        {
            goldText.SetText("{0}", InGameState.Golds);
            rockText.SetText("{0}", InGameState.Rocks);
            woodText.SetText("{0}", InGameState.Woods);
            livesText.SetText("{0}", InGameState.Lives);
            waveText.SetText("{0}", InGameState.WaveCount);
            gameSpeedText.SetText($"x{InGameState.GameSpeed}");
        }

        private void HandleSettingsClick() => PauseGame();
        public void OnCloseSettingsClick() => ResumeGame();
        private void HandleStartWaveClick() => SpawnManager.Instance.StartWave();

        private void HandleGameSpeedClick()
        {
            InGameState.GameSpeed++;
        }

        public void PauseGame() => GameEvent.PauseGame();
        public void ResumeGame() => GameEvent.ResumeGame();
    }
}
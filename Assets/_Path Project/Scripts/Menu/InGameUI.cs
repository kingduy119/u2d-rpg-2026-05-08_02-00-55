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

        private InGameState InGame => GameManager.Instance.InGame;

        private void Awake()
        {
            settingsPanel.SetActive(false);
            gameSpeedText = gameSpeedButton.GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            if (InGame != null)
            {
                Enemy.OnEnemyReachedEnd += InGame.HandlePointReachedEnd;
                Enemy.OnGetEnemyReward += InGame.HandleGetEnemyReward;
                SpawnManager.OnWaveChanged += InGame.HandleWaveChanged;
            }

            settingsButton.onClick.AddListener(HandleSettingsClick);
            startWaveButton.onClick.AddListener(HandleStartWaveClick);
            gameSpeedButton.onClick.AddListener(HandleGameSpeedClick);

            GameEvent.OnLoadLevel += LoadLevelResource;
        }

        private void OnDisable()
        {
            if (InGame != null)
            {
                Enemy.OnEnemyReachedEnd -= InGame.HandlePointReachedEnd;
                Enemy.OnGetEnemyReward -= InGame.HandleGetEnemyReward;
                SpawnManager.OnWaveChanged -= InGame.HandleWaveChanged;
            }


            settingsButton.onClick.RemoveListener(HandleSettingsClick);
            startWaveButton.onClick.RemoveListener(HandleStartWaveClick);
            gameSpeedButton.onClick.RemoveListener(HandleGameSpeedClick);

            GameEvent.OnLoadLevel -= LoadLevelResource;
        }

        private void Update()
        {
            if (InGame.IsDirty)
            {
                UpdateInGameUI();
                InGame.MarkDirty();
            }

        }

        private void LoadLevelResource()
        {
            LevelSO level = LevelManager.Instance.LevelSO;
            InGame.Golds = level.startingGold;
            InGame.Lives = level.startingLives;
        }

        private void UpdateInGameUI()
        {
            goldText.SetText("{0}", InGame.Golds);
            rockText.SetText("{0}", InGame.Rocks);
            woodText.SetText("{0}", InGame.Woods);
            livesText.SetText("{0}", InGame.Lives);
            waveText.SetText("{0}", InGame.WaveCount);
            gameSpeedText.SetText($"x{InGame.GameSpeed}");
        }

        private void HandleSettingsClick() => PauseGame();

        public void OnCloseSettingsClick() => ResumeGame();

        private void HandleStartWaveClick()
        {
            SpawnManager.Instance.StartWave();
        }

        private void HandleGameSpeedClick()
        {
            InGame.GameSpeed++;
        }

        public void PauseGame() => GameEvent.PauseGame();
        public void ResumeGame() => GameEvent.ResumeGame();
    }
}
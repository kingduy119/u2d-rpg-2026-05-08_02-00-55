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


        private InGameController m_controller;

        private void Awake()
        {
            settingsPanel.SetActive(false);
            m_controller = new();
        }

        private void OnEnable()
        {
            Enemy.OnEnemyReachedEnd += m_controller.HandlePointReachedEnd;
            Enemy.OnGetEnemyReward += m_controller.HandleGetEnemyReward;
            SpawnManager.OnWaveChanged += m_controller.HandleWaveChanged;

            settingsButton.onClick.AddListener(HandleSettingsClick);
            startWaveButton.onClick.AddListener(HandleStartWaveClick);
            gameSpeedButton.onClick.AddListener(HandleGameSpeedClick);

            GameEvent.OnLoadLevel += LoadLevelResource;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyReachedEnd -= m_controller.HandlePointReachedEnd;
            Enemy.OnGetEnemyReward -= m_controller.HandleGetEnemyReward;
            SpawnManager.OnWaveChanged -= m_controller.HandleWaveChanged;

            settingsButton.onClick.RemoveListener(HandleSettingsClick);
            startWaveButton.onClick.RemoveListener(HandleStartWaveClick);
            gameSpeedButton.onClick.RemoveListener(HandleGameSpeedClick);

            GameEvent.OnLoadLevel -= LoadLevelResource;
        }

        private void Update()
        {
            if (m_controller.IsDirty)
            {
                UpdateInGameUI();
                m_controller.MarkDirty();
            }

        }

        private void LoadLevelResource()
        {
            LevelSO level = LevelManager.Instance.LevelSO;
            m_controller.Golds = level.startingGold;
            m_controller.Lives = level.startingLives;
        }

        private void UpdateInGameUI()
        {
            goldText.SetText("{0}", m_controller.Golds);
            rockText.SetText("{0}", m_controller.Rocks);
            woodText.SetText("{0}", m_controller.Woods);
            livesText.SetText("{0}", m_controller.Lives);
            waveText.SetText("{0}", m_controller.WaveCount);
            gameSpeedButton.GetComponentInChildren<TMP_Text>().SetText($"x{m_controller.GameSpeed}");
        }

        private void HandleSettingsClick() => PauseGame();
        public void OnCloseSettingsClick() => ResumeGame();

        private void HandleStartWaveClick()
        {
            SpawnManager.Instance.StartWave();
        }

        private void HandleGameSpeedClick()
        {
            m_controller.GameSpeed++;
        }

        public void PauseGame() => GameEvent.PauseGame();
        public void ResumeGame() => GameEvent.ResumeGame();
    }
}
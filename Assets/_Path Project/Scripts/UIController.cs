using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace TDGame
{
    public class UIController : PersistentSingleton<UIController>
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
        [SerializeField] private Button gameSpeedButton;
        [SerializeField] private Button startWaveButton;
        private TMP_Text gameSpeedText;

        [Header("UI Pannels")]
        [SerializeField] private GameObject towerPanel;
        [SerializeField] private GameObject missionCompletePanel;

        [Header("Others")]
        [SerializeField] private GameObject towerCardPrefab;
        [SerializeField] private Transform cardsContainer;
        [SerializeField] private TowerData[] _towers;


        private bool _isPaused = false;
        private float _gameSpeed = 1f;
        private float _maxGameSpeed = 3f;
        public float GameSpeed => _gameSpeed;
        private Platform _currentPlatform;

        protected override void Awake()
        {
            base.Awake();
            gameSpeedText = gameSpeedButton.GetComponentInChildren<TMP_Text>();
        }

        void OnEnable()
        {
            TDGameManager.OnLivesChanged += UpdateLives;
            TDGameManager.OnGoldsChanged += UpdateGolds;
            SpawnManager.OnWaveChanged += UpdateWaveText;
            SpawnManager.OnMissionComplete += HandleMissionComplete;

            Platform.OnPlatformClicked += OpenTowerPanel;
            TowerCard.OnTowerCardSelected += HandleTowerCardSelected;
        }

        void OnDisable()
        {
            TDGameManager.OnLivesChanged -= UpdateLives;
            TDGameManager.OnGoldsChanged -= UpdateGolds;
            SpawnManager.OnWaveChanged -= UpdateWaveText;
            SpawnManager.OnMissionComplete -= HandleMissionComplete;

            Platform.OnPlatformClicked -= OpenTowerPanel;
            TowerCard.OnTowerCardSelected -= HandleTowerCardSelected;
        }

        void Start()
        {
            towerPanel.SetActive(false);
            missionCompletePanel.SetActive(false);

            UpdateGameSpeedUI();
            HideAlert();
            ResetUI();

            gameSpeedButton.onClick.AddListener(OnGameSpeedButtonClicked);
        }

        private void ResetUI()
        {
            UpdateGolds(0);
            UpdateRocks(0);
            UpdateWoods(0);
            UpdateLives(0);
            UpdateEnemies(0);
        }

        public void UpdateGolds(int number) => goldText.SetText("{0}", number);
        public void UpdateRocks(int number) => rockText.SetText("{0}", number);
        public void UpdateWoods(int number) => woodText.SetText("{0}", number);
        public void UpdateLives(int number) => livesText.SetText("{0}", number);
        public void UpdateEnemies(int number) => enemiesText.SetText("{0}", number);
        private void UpdateWaveText(int waveIndex, int total)
        {
            waveText.text = $"{waveIndex + 1}/{total}";
            UpdateStartWaveButton();
        }

        public void UpdateGameSpeedUI() => gameSpeedText.text = $"x{_gameSpeed}";
        public void UpdateStartWaveButton() => startWaveButton.interactable = !SpawnManager.Instance.ActiveWave;
        private void HideAlert() => alertText.gameObject.SetActive(false);


        public void ToggleTowerPanel()
        {
            towerPanel.SetActive(!towerPanel.activeSelf);
            if (towerPanel.activeSelf)
                PopulateTowerCards();

            TogglePause();
        }

        public void OpenTowerPanel(Platform platform)
        {
            if (towerPanel.activeSelf) return;
            towerPanel.SetActive(true);

            _currentPlatform = platform;
            PopulateTowerCards();
            UpdateStartWaveButton();

            Pause();
        }

        public void CloseTowerPanel()
        {
            towerPanel.SetActive(false);
            UpdateStartWaveButton();
            Resume();
        }

        public void ShowAlert(string message)
        {
            alertText.text = message;
            alertText.gameObject.SetActive(true);
        }

        private void ResetTowerCards()
        {
            foreach (Transform child in cardsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void FillTowerCards()
        {
            foreach (var data in _towers)
            {
                GameObject card = Instantiate(towerCardPrefab, cardsContainer);
                TowerCard towerCard = card.GetComponent<TowerCard>();
                towerCard.Initialize(data);
            }
        }
        private void PopulateTowerCards()
        {
            ResetTowerCards();
            FillTowerCards();
        }

        public void HandleTowerCardSelected(TowerData data)
        {
            if (TDGameManager.Instance.Golds < data.cost)
            {
                StartCoroutine(ShowAlertCoroutine("Not enough gold!"));
                return;
            }
            if (_currentPlatform != null)
            {
                TDGameManager.Instance.SpendGold(data.cost);
                _currentPlatform.PlaceTower(data);
                ToggleTowerPanel();
            }
        }

        private System.Collections.IEnumerator ShowAlertCoroutine(string message)
        {
            ShowAlert(message);
            yield return new WaitForSeconds(2f);
            HideAlert();
        }

        public void SetGameSpeed(float speed)
        {
            _gameSpeed = Mathf.Clamp(speed, 1f, _maxGameSpeed);
            TDGameManager.Instance.SetTimeScale(_gameSpeed);
            UpdateGameSpeedUI();
        }

        public void OnGameSpeedButtonClicked()
        {
            _gameSpeed = (_gameSpeed + 1) % (_maxGameSpeed + 1);
            SetGameSpeed(_gameSpeed);
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            TDGameManager.Instance.SetTimeScale(_isPaused ? 0f : _gameSpeed);
            if (_isPaused)
                AudioManager.Instance.PlayPauseSound();
            else
                AudioManager.Instance.PlayResumeSound();

        }

        public void Pause()
        {
            _isPaused = true;
            TDGameManager.Instance.SetTimeScale(0f);
            AudioManager.Instance.PlayPauseSound();
        }

        public void Resume()
        {
            _isPaused = false;
            TDGameManager.Instance.SetTimeScale(_gameSpeed);
            AudioManager.Instance.PlayResumeSound();
        }

        public void OnStartNewWave()
        {
            SpawnManager.Instance.StartNewWave();
            UpdateStartWaveButton();
        }

        private void HandleMissionComplete()
        {
            Debug.Log($"missionCompletePanel: {missionCompletePanel.activeSelf}");
            missionCompletePanel.SetActive(true);
            Pause();
        }

        public void OnNextLevelClick()
        {
            missionCompletePanel.SetActive(false);
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
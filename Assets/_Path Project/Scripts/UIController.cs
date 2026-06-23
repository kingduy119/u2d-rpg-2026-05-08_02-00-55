using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace TDGame
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; set; }
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text alertText;
        [SerializeField] private Button gameSpeedButton;
        [SerializeField] private Button startWaveButton;

        [SerializeField] private GameObject towerPanel;
        [SerializeField] private GameObject towerCardPrefab;
        [SerializeField] private Transform cardsContainer;

        [SerializeField] private GameObject missionCompletePanel;

        [SerializeField] private TowerData[] _towers;
        // private List<GameObject> activeCards = new List<GameObject>();

        private Platform _currentPlatform;

        private bool _isPaused = false;
        private float _gameSpeed = 1f;
        private float _maxGameSpeed = 3f;
        public float GameSpeed => _gameSpeed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void OnEnable()
        {
            SpawnManager.OnWaveChanged += UpdateWaveText;
            SpawnManager.OnMissionComplete += HandleMissionComplete;
            TDGameManager.OnLivesChanged += UpdateLives;
            TDGameManager.OnGoldsChanged += UpdateGolds;
            Platform.OnPlatformClicked += OpenTowerPanel;
            TowerCard.OnTowerCardSelected += HandleTowerCardSelected;
        }

        void OnDisable()
        {
            SpawnManager.OnWaveChanged -= UpdateWaveText;
            SpawnManager.OnMissionComplete -= HandleMissionComplete;
            TDGameManager.OnLivesChanged -= UpdateLives;
            TDGameManager.OnGoldsChanged -= UpdateGolds;
            Platform.OnPlatformClicked -= OpenTowerPanel;
            TowerCard.OnTowerCardSelected -= HandleTowerCardSelected;
        }

        void Start()
        {
            towerPanel.SetActive(false);
            missionCompletePanel.SetActive(false);

            UpdateGameSpeedUI();
            HideAlert();

            gameSpeedButton.onClick.AddListener(OnGameSpeedButtonClicked);
            // startWaveButton.onClick.AddListener(OnStartNewWave);
        }

        private void UpdateWaveText(int waveIndex)
        {
            waveText.text = "Wave " + (waveIndex + 1);
            UpdateStartWaveButton();
        }

        public void UpdateLives(int lives)
        {
            livesText.text = "Lives: " + lives;
        }

        public void UpdateGolds(int gold)
        {
            goldText.text = "Gold: " + gold;
        }

        public void UpdateGameSpeedUI()
        {
            gameSpeedButton.GetComponentInChildren<TMP_Text>().text = "x " + _gameSpeed;
        }

        public void UpdateStartWaveButton()
        {
            startWaveButton.interactable = !SpawnManager.Instance.ActiveWave;
        }

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

        private void HideAlert()
        {
            alertText.gameObject.SetActive(false);
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
            missionCompletePanel.SetActive(true);

            Pause();
        }

        public void OnNextLevelClick()
        {
            missionCompletePanel.SetActive(false);
            var levelManager = LevelManager.Instance;
            int currentIndex = Array.IndexOf(levelManager.allLevels, levelManager.CurrentLevel);
            int nextIndex = currentIndex + 1;
            Debug.Log($"OnNextLevelClick-nextIndex: {nextIndex} - {levelManager.allLevels.Length}");
            if (nextIndex < levelManager.allLevels.Length)
            {
                levelManager.LoadLevel(levelManager.allLevels[nextIndex]);
            }
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
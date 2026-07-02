using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        [SerializeField] private GameObject missionCompletePanel;


        private void Awake()
        {
            settingsPanel.SetActive(false);
            missionCompletePanel.SetActive(false);
        }

        private void OnEnable()
        {
            InGameController.OnUpdateInGameUI += UpdateInGameUI;

            settingsButton.onClick.AddListener(HandleSettingsClick);
            startWaveButton.onClick.AddListener(HandleStartWaveClick);
            gameSpeedButton.onClick.AddListener(HandleGameSpeedClick);
        }

        private void OnDisable()
        {
            InGameController.OnUpdateInGameUI -= UpdateInGameUI;

            settingsButton.onClick.RemoveListener(HandleSettingsClick);
            startWaveButton.onClick.RemoveListener(HandleStartWaveClick);
            gameSpeedButton.onClick.RemoveListener(HandleGameSpeedClick);
        }

        private void UpdateInGameUI()
        {
            InGameController inGame = GameManager.Instance.InGame;
            goldText.SetText("{0}", inGame.Golds);
            rockText.SetText("{0}", inGame.Rocks);
            woodText.SetText("{0}", inGame.Woods);
            livesText.SetText("{0}", inGame.Lives);
            gameSpeedButton.GetComponentInChildren<TMP_Text>().SetText($"x{inGame.GameSpeed}");
        }

        private void HandleSettingsClick()
        {
            PauseGame();
        }

        public void OnCloseSettingsClick()
        {
            ResumeGame();
        }

        private void HandleStartWaveClick()
        {

        }

        private void HandleGameSpeedClick()
        {
            GameManager.Instance.InGame.GameSpeed++;
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

        // public void UpdateGameSpeedUI() => gameSpeedText.text = $"x{_gameSpeed}";
        public void UpdateStartWaveButton() => startWaveButton.interactable = !SpawnManager.Instance.ActiveWave;
        private void HideAlert() => alertText.gameObject.SetActive(false);

        public void PauseGame() => GameEvent.PauseGame();
        public void ResumeGame() => GameEvent.ResumeGame();
    }
}
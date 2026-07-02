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
        [SerializeField] private Button gameSpeedButton;
        [SerializeField] private Button startWaveButton;
        private TMP_Text gameSpeedText;

        private void OnEnable()
        {
            InGameController.OnUpdateInGameUI += UpdateInGameUI;
        }

        private void OnDisable()
        {
            InGameController.OnUpdateInGameUI -= UpdateInGameUI;
        }

        private void UpdateInGameUI()
        {
            InGameController inGame = TDGameManager.Instance.InGame;
            goldText.SetText("{0}", inGame.Golds);
            rockText.SetText("{0}", inGame.Rocks);
            woodText.SetText("{0}", inGame.Woods);
            livesText.SetText("{0}", inGame.Lives);
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

        public void OnPause() => GameEvent.OnPauseInGame?.Invoke();
        public void OnResume() => GameEvent.OnResumeInGame?.Invoke();
    }
}
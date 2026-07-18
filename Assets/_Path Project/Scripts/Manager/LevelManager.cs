using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class LevelManager : PersistentSingleton<LevelManager>
    {
        public LevelState LevelState;

        private void OnEnable()
        {
            GameEvent.OnPlayAgain += PlayAgain;
            GameEvent.OnPlaynewGame += LoadLevel;
            GameEvent.OnMissionComplete += HandleMissionComplete;
        }

        private void OnDisable()
        {
            GameEvent.OnPlayAgain -= PlayAgain;
            GameEvent.OnPlaynewGame -= LoadLevel;
            GameEvent.OnMissionComplete -= HandleMissionComplete;
        }

        private void Start()
        {
            GameEvent.UpdateUI();
            LevelState.LoadLevelData();
        }

        public void LoadLevel(int level)
        {
            LevelState.Level = level;
            SceneManager.LoadScene(LevelState.CurrentLevel.sceneName);
        }

        public void PlayAgain()
        {
            LoadLevel(LevelState.Level);
        }

        public void HandleMissionComplete()
        {
            LevelState.Level++;
            LevelState.SaveLevelData();
        }
    }
}
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
            GameEvent.OnPlayAgain += PlayCurrentLevel;
            GameEvent.OnPlaynewGame += LoadLevel;
            GameEvent.OnMissionComplete += HandleMissionComplete;
        }

        private void OnDisable()
        {
            GameEvent.OnPlayAgain -= PlayCurrentLevel;
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

        public void PlayCurrentLevel()
        {
            SceneManager.LoadScene(LevelState.CurrentLevel.sceneName);
        }

        public void HandleMissionComplete()
        {
            Debug.Log($"HandleMissionComplete:1 {LevelState.Level}");
            LevelState.Level++;
            LevelState.SaveLevelData();
            Debug.Log($"HandleMissionComplete:2 {LevelState.Level}");
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class LevelManager : MonoBehaviour
    {
        public LevelState LevelState;

        private void OnEnable()
        {
            GameEvent.OnPlayAgain += PlayCurrentLevel;
            GameEvent.OnPlaynewGame += LoadLevel;

            InGameEvent.OnMissionComplete += HandleMissionComplete;
        }

        private void OnDisable()
        {
            GameEvent.OnPlayAgain -= PlayCurrentLevel;
            GameEvent.OnPlaynewGame -= LoadLevel;

            InGameEvent.OnMissionComplete -= HandleMissionComplete;
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
            LevelState.Level++;
            LevelState.SaveLevelData();
        }
    }
}
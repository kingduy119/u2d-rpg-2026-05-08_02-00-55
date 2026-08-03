using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class LevelManager : MonoBehaviour
    {
        public LevelState LevelState;

        private int _Spawners = 0;
        private int _PathwaySpawners = 0;

        private void OnEnable()
        {
            InGameEvent.OnPathwayRaise += HandlePathwayRaise;
            InGameEvent.OnSpawnerRaise += HandleSpawnerRaise;
            InGameEvent.OnMissionComplete += HandleMissionComplete;
        }

        private void OnDisable()
        {
            InGameEvent.OnPathwayRaise -= HandlePathwayRaise;
            InGameEvent.OnSpawnerRaise -= HandleSpawnerRaise;
            InGameEvent.OnMissionComplete -= HandleMissionComplete;
        }

        private void HandlePathwayRaise()
        {
            _PathwaySpawners++;
        }

        private void HandleSpawnerRaise()
        {
            _Spawners++;
        }

        private void Start()
        {
            GameEvent.UpdateUI();
            LevelState.LoadLevelData();
        }

        public void LoadLevel(int level)
        {
            LevelState.Level = level;
            InGameEvent.OnLevelLoaded?.Invoke(LevelState.CurrentLevel);
            SceneManager.LoadScene(LevelState.CurrentLevel.sceneName);
        }

        public void PlayContinueLevel()
        {
            InGameEvent.OnLevelLoaded?.Invoke(LevelState.CurrentLevel);
            SceneManager.LoadScene(LevelState.CurrentLevel.sceneName);
        }

        public void HandleMissionComplete()
        {
            LevelState.Level++;
            LevelState.SaveLevelData();
        }
    }
}
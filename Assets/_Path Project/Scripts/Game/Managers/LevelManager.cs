using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class LevelManager
    {
        public LevelState LevelState = new();

        private int _Spawners = 0;
        private int _PathwaySpawners = 0;

        public LevelManager()
        {
            LevelState.LoadLevelData();
        }

        public void Enable()
        {
            InGameEvent.PathwayStart += OnPathwayStart;
            InGameEvent.PathwayEnd += OnPathwayEnd;
            InGameEvent.SpawnerStart += OnSpawnStart;
            InGameEvent.SpawnerEnd += OnSpawnerEnd;
        }

        public void Disable()
        {
            InGameEvent.PathwayStart -= OnPathwayStart;
            InGameEvent.PathwayEnd -= OnPathwayEnd;
            InGameEvent.SpawnerStart -= OnSpawnStart;
            InGameEvent.SpawnerEnd -= OnSpawnerEnd;
        }

        private void OnPathwayStart() { _PathwaySpawners++; }
        private void OnSpawnStart() { _Spawners++; }
        private void OnPathwayEnd()
        {
            _PathwaySpawners--;
            CheckComplete();
        }

        private void OnSpawnerEnd()
        {
            _Spawners--;
            CheckComplete();
        }

        private void CheckComplete()
        {
            if (_Spawners <= 0 && _PathwaySpawners <= 0)
            {
                HandleMissionComplete();
                InGameEvent.MissionComplete?.Invoke();
            }
            else if (_Spawners <= 0)
            {
                InGameEvent.WaveCompleted?.Invoke();
            }
        }

        public void LoadLevel(int level)
        {
            LevelState.Level = level;
            // InGameEvent.OnLevelLoaded?.Invoke(LevelState.CurrentLevel);
            Addressables.LoadSceneAsync("LoadingScene", activateOnLoad: true);
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
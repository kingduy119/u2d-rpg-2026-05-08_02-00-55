using UnityEngine;
using UnityEngine.AddressableAssets;
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
            InGameEvent.PathwayStart += PathwayStart;
            InGameEvent.PathwayEnd += PathwayEnd;
            InGameEvent.SpawnerStart += SpawnStart;
            InGameEvent.SpawnerEnd += SpawnerEnd;
        }

        private void OnDisable()
        {
            InGameEvent.PathwayStart -= PathwayStart;
            InGameEvent.PathwayEnd -= PathwayEnd;
            InGameEvent.SpawnerStart -= SpawnStart;
            InGameEvent.SpawnerEnd -= SpawnerEnd;
        }

        private void PathwayStart() { _PathwaySpawners++; }
        private void SpawnStart() { _Spawners++; }
        private void PathwayEnd()
        {
            _PathwaySpawners--;
            CheckComplete();
        }
        private void SpawnerEnd()
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

        private void Start()
        {
            LevelState.LoadLevelData();
        }

        public void LoadLevel(int level)
        {
            LevelState.Level = level;
            InGameEvent.OnLevelLoaded?.Invoke(LevelState.CurrentLevel);
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
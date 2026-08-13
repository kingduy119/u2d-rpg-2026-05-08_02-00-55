using UnityEngine;

namespace TDGame
{
    public class LevelManager : MonoBehaviour
    {
        public LevelState LevelState = new();

        private int _Spawners = 0;
        private int _PathwaySpawners = 0;

        public LevelManager(GameManager gm)
        {
            LevelState.LoadLevelData();
        }

        public void OnEnable()
        {
            GamePlayEvent.PathwayStart += OnPathwayStart;
            GamePlayEvent.PathwayEnd += OnPathwayEnd;
            GamePlayEvent.SpawnerStart += OnSpawnStart;
            GamePlayEvent.SpawnerEnd += OnSpawnerEnd;

            GamePlayEvent.RequestLevelResource += OnRequestLevelResource;
        }

        public void OnDisable()
        {
            GamePlayEvent.PathwayStart -= OnPathwayStart;
            GamePlayEvent.PathwayEnd -= OnPathwayEnd;
            GamePlayEvent.SpawnerStart -= OnSpawnStart;
            GamePlayEvent.SpawnerEnd -= OnSpawnerEnd;

            GamePlayEvent.RequestLevelResource -= OnRequestLevelResource;
        }

        private void OnRequestLevelResource() => GamePlayEvent.ResponseLevelResource?.Invoke(LevelState.CurrentLevel);

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
                GamePlayEvent.MissionComplete?.Invoke();
            }
            else if (_Spawners <= 0)
            {
                GamePlayEvent.WaveCompleted?.Invoke();
            }
        }

        public void LoadLevel(int level)
        {
            LevelState.Level = level;
            GameEvent.LoadScene?.Invoke(LevelState.CurrentLevel.sceneName);

        }

        public void PlayContinueLevel()
        {
            // GamePlayEvent.OnLevelLoaded?.Invoke(LevelState.CurrentLevel);
            GameEvent.LoadScene?.Invoke(LevelState.CurrentLevel.sceneName);
        }

        public void HandleMissionComplete()
        {
            LevelState.Level++;
            LevelState.SaveLevelData();
        }
    }
}
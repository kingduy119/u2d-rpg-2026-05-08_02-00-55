
using System;
using UnityEngine;

namespace TDGame
{
    [Serializable]
    public class LevelState : DirtyState
    {
        private const string SaveKey = "LevelState";

        private int _Spawners = 0;
        private int _PathwaySpawners = 0;
        private int _Enemies = 0;

        public int Level { get; set; } = 0;
        public int CompletedLevel = 0;
        public int MaxLevel;

        public LevelState(int maxLevel = 0)
        {
            MaxLevel = maxLevel;
        }

        public void OnEnable()
        {
            GamePlayEvent.PathwayCount += GamePlayEvent_PathwayCount;
            GamePlayEvent.PathwayEnd += GamePlayEvent_PathwayEnd;
            GamePlayEvent.SpawnerCount += OnSpawnerCount;
            GamePlayEvent.SpawnerEnd += OnSpawnerEnd;

            EnemyEvent.EnemySpawn += EnemyEvent_EnemySpawn;
            EnemyEvent.EnemyDie += EnemyEvent_EnemyDie;
            EnemyEvent.ReachedEnd += EnemyEvent_ReachedEnd;
        }

        public void OnDisable()
        {
            GamePlayEvent.PathwayCount -= GamePlayEvent_PathwayCount;
            GamePlayEvent.PathwayEnd -= GamePlayEvent_PathwayEnd;
            GamePlayEvent.SpawnerCount -= OnSpawnerCount;
            GamePlayEvent.SpawnerEnd -= OnSpawnerEnd;

            EnemyEvent.EnemySpawn -= EnemyEvent_EnemySpawn;
            EnemyEvent.EnemyDie -= EnemyEvent_EnemyDie;
            EnemyEvent.ReachedEnd -= EnemyEvent_ReachedEnd;
        }

        private void EnemyEvent_EnemyDie(Enemy _) => CheckWaveAndMissionComplete();
        private void EnemyEvent_ReachedEnd(Enemy _) => CheckWaveAndMissionComplete();
        private void GamePlayEvent_PathwayCount() { _PathwaySpawners++; }
        private void GamePlayEvent_PathwayEnd() { _PathwaySpawners--; }
        private void OnSpawnerCount() { _Spawners++; }
        private void OnSpawnerEnd() { _Spawners--; }
        private void EnemyEvent_EnemySpawn() => _Enemies++;

        private void CheckWaveAndMissionComplete()
        {
            _Enemies--;
            Debug.Log($"_PathwaySpawners: {_PathwaySpawners} _Spawners: {_Spawners} _Enemies: {_Enemies}");
            if (_Spawners <= 0 && _Enemies <= 0 && _PathwaySpawners <= 0)
            {
                HandleMissionComplete();
                GamePlayEvent.MissionComplete?.Invoke();
            }
            else if (_Spawners <= 0 && _Enemies <= 0)
            {
                GamePlayEvent.WaveCompleted?.Invoke();
            }
        }

        public void HandleMissionComplete()
        {
            Level++;
            SaveLevelData();
        }

        // ##########

        public void UpLevel() => Level = Mathf.Clamp(Level + 1, 0, MaxLevel);

        public void SaveLevelData()
        {
            PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(new LevelData(Level)));
            PlayerPrefs.Save();
        }

        public void LoadSaveDate()
        {
            if (!PlayerPrefs.HasKey(SaveKey)) return;
            var data = JsonUtility.FromJson<LevelData>(PlayerPrefs.GetString(SaveKey));
            CompletedLevel = data.level;
        }
    }
}
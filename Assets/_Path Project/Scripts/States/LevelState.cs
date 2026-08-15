
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

        public int Level { get; set; } = 0;
        public int CompletedLevel = 0;
        public int MaxLevel;

        public LevelState(int maxLevel = 0)
        {
            MaxLevel = maxLevel;
        }

        public void OnEnable()
        {
            GamePlayEvent.PathwayCount += OnPathwayCount;
            GamePlayEvent.PathwayEnd += OnPathwayEnd;
            GamePlayEvent.SpawnerCount += OnSpawnerCount;
            GamePlayEvent.SpawnerEnd += OnSpawnerEnd;
        }

        public void OnDisable()
        {
            GamePlayEvent.PathwayCount -= OnPathwayCount;
            GamePlayEvent.PathwayEnd -= OnPathwayEnd;
            GamePlayEvent.SpawnerCount -= OnSpawnerCount;
            GamePlayEvent.SpawnerEnd -= OnSpawnerEnd;
        }

        private void OnPathwayCount() { _PathwaySpawners++; }
        private void OnSpawnerCount() { _Spawners++; }
        private void OnPathwayEnd()
        {
            _PathwaySpawners--;
            CheckWaveAndMissionComplete();
        }

        private void OnSpawnerEnd()
        {
            _Spawners--;
            CheckWaveAndMissionComplete();
        }

        private void CheckWaveAndMissionComplete()
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
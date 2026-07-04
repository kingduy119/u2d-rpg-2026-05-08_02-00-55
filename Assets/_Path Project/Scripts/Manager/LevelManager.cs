using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class LevelManager : PersistentSingleton<LevelManager>
    {
        private int _level = 0;

        public LevelSO[] allLevels;
        public LevelSO LevelSO => allLevels[_level];

        public int Level
        {
            get => _level;
            set
            {
                _level = Mathf.Clamp(value, 0, allLevels.Length - 1);
            }
        }

        private void OnEnable()
        {
            SpawnManager.OnMissionComplete += HandleMissionComplete;
        }

        private void OnDisable()
        {
            SpawnManager.OnMissionComplete -= HandleMissionComplete;
        }

        private void Start()
        {
            LoadLevelData();
            GameEvent.UpdateUI();
        }

        public void LoadLevel(int level)
        {
            _level = level;
            SceneManager.LoadScene(LevelSO.sceneName);
        }

        public void PlayContinue()
        {
            LoadLevel(Level);
        }

        public void HandleMissionComplete()
        {
            Level++;
            SaveLevelData();
        }

        private void SaveLevelData()
        {
            string json = JsonUtility.ToJson(new LevelData(_level));
            PlayerPrefs.SetString("Level", json);

            PlayerPrefs.Save();
        }

        private void LoadLevelData()
        {
            if (!PlayerPrefs.HasKey("Level")) return;

            string json = PlayerPrefs.GetString("Level");
            LevelData data = JsonUtility.FromJson<LevelData>(json);
            Level = data.level;

        }
    }
}
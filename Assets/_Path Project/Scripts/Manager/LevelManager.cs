using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class LevelManager : PersistentSingleton<LevelManager>
    {
        private int _level = 0;

        public LevelSO[] allLevels;
        public LevelSO Level => allLevels[_level];
        public WaveData[] Waves => allLevels[_level].waves;

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

            TDGameManager.Instance.Golds += Level.startingGold;
        }

        public void LoadLevel(int level)
        {
            _level = level;
            SceneManager.LoadScene(Level.sceneName);
        }

        public void PlayContinue()
        {
            LoadLevel(_level);
        }

        public void HandleMissionComplete()
        {
            _level++;
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
            _level = data.level;
        }
    }
}
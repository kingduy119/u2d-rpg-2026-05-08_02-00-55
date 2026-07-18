
using System;
using UnityEngine;

namespace TDGame
{
    [Serializable]
    public class LevelState : DirtyState
    {
        private const string SaveKey = "LevelState";

        // private int Level = 0;
        public int Level { get; set; } = 0;

        public LevelSO[] allLevels;
        public LevelSO CurrentLevel => allLevels[Level];

        public void UpLevel() => Level = Mathf.Clamp(Level + 1, 0, allLevels.Length - 1);

        public void SaveLevelData()
        {
            PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(new LevelData(Level)));
            PlayerPrefs.Save();
        }

        public void LoadLevelData()
        {
            if (!PlayerPrefs.HasKey(SaveKey)) return;
            var data = JsonUtility.FromJson<LevelData>(PlayerPrefs.GetString(SaveKey));
            Level = data.level;
        }
    }
}
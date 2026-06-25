using System;
using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "LevelSO", menuName = "Game TD/LevelSO")]
    public class LevelSO : ScriptableObject
    {
        public string levelName;
        public string sceneName;
        public int startingLives;
        public int startingGold;

        public WaveData[] waves;
    }


    [Serializable]
    public class LevelData
    {
        public int level;

        public LevelData(int level)
        {
            this.level = level;
        }
    }
}
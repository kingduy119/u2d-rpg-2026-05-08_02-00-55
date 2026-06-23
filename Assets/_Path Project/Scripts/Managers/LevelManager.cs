using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public LevelData[] allLevels;
        public LevelData CurrentLevel { get; private set; }
        private int level = 0;
        public LevelData Level => allLevels[level];

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            CurrentLevel = allLevels[0];
            TDGameManager.Instance.AddGold(CurrentLevel.startingGold);
            Debug.Log($"allLevels.Length: {allLevels.Length}");
        }

        public void LoadLevel(LevelData levelData)
        {
            CurrentLevel = levelData;
            Debug.Log($"Loading level: {CurrentLevel.sceneName}");
            SceneManager.LoadScene(CurrentLevel.sceneName);
        }

        public void LoadNewgame()
        {
            LoadLevel(allLevels[0]);
        }
    }

}
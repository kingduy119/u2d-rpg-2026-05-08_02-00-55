using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public LevelData currentLevel;
    public LevelData[] allLevels;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        currentLevel = allLevels[0];
    }

    public void LoadLevel(LevelData levelData)
    {
        // Load the scene associated with the level data
        SceneManager.LoadScene(levelData.sceneName);


    }
}

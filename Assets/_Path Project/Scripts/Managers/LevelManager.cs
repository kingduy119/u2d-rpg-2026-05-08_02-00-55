using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public LevelData CurrentLevel { get; set; }
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
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        CurrentLevel = allLevels[0];
        TDGameManager.Instance.AddGold(CurrentLevel.startingGold);
    }

    public void LoadLevel(LevelData levelData)
    {
        CurrentLevel = levelData;
        SceneManager.LoadScene(levelData.sceneName);
    }
}

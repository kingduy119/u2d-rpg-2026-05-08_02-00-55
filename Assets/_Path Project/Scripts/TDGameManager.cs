using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TDGameManager : MonoBehaviour
{
    public static TDGameManager Instance { get; set; }
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnGoldsChanged;

    private int _lives = 20;
    public int Lives => _lives;

    private int _golds = 0;

    public int Golds => _golds;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    void OnEnable()
    {
        TDEnemy.OnEnemyReachedEnd += HandlePointReachedEnd;
        TDEnemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        TDEnemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
        TDEnemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        OnLivesChanged?.Invoke(_lives);
        OnGoldsChanged?.Invoke(_golds);
    }

    private void HandlePointReachedEnd(TDEnemyData pointData)
    {
        _lives -= pointData.damage;
        OnLivesChanged?.Invoke(_lives);

        if (_lives <= 0)
        {
            Debug.Log("Game Over!");
            // Implement game over logic here (e.g., show game over screen, restart level, etc.)
        }
    }

    private void HandleEnemyDestroyed(TDEnemy enemy)
    {
        AddGold(enemy.Data.goldReward);
    }

    public void AddGold(int gold)
    {
        _golds += gold;
        OnGoldsChanged?.Invoke(_golds);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SpendGold(int amount)
    {
        if (_golds >= amount)
        {
            _golds -= amount;
            OnGoldsChanged?.Invoke(_golds);
        }
        else
        {
            Debug.LogWarning("Not enough gold!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TD_MainMenu")
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        }
        else if (LevelManager.Instance != null && LevelManager.Instance.CurrentLevel != null)
        {
            // ResetGameState();
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameplayMusic);
        }
    }
}

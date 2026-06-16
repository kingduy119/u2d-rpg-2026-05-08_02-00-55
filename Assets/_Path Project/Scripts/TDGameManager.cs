using UnityEngine;
using System;

public class TDGameManager : MonoBehaviour
{
    public static TDGameManager Instance { get; set; }
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnGoldsChanged;

    private int _lives = 20;
    private int _golds = 0;

    public int Lives => _lives;
    public int Golds => _golds;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        TDEnemy.OnEnemyReachedEnd += HandlePointReachedEnd;
        TDEnemy.OnEnemyDestroyed += HandleEnemyDestroyed;

    }

    void OnDisable()
    {
        TDEnemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
        TDEnemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
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
}

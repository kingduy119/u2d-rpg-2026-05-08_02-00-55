using UnityEngine;
using System;

public class TDGameManager : MonoBehaviour
{
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnGoldsChanged;

    private int _lives = 20;
    private int _golds = 0;


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
}

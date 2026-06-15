using UnityEngine;
using System;

public class TDGameManager : MonoBehaviour
{
    public static event Action<int> OnLivesChanged;
    private int _lives = 20;


    void OnEnable()
    {
        TDEnemy.OnEnemyReachedEnd += HandlePointReachedEnd;
    }

    void OnDisable()
    {
        TDEnemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
    }

    void Start()
    {
        OnLivesChanged?.Invoke(_lives);
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
}

using System;
using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Game TD/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public event Action HealthChanged;

        public EnemyType type;
        public Enemy prefab;


        [Header("Detail")]
        public float lives;
        public float minLives;
        public float maxLives;
        public int damage;
        public float moveSpeed;
        public int goldReward;

        public void TakeDamge(float amount)
        {
            lives -= amount;
            lives = Mathf.Clamp(lives, minLives, maxLives);

            HealthChanged?.Invoke();
        }
    }
}

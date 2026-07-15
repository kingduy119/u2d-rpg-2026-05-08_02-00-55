using System;
using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Game TD/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        public EnemyType type;
        public Enemy prefab;


        [Header("Health")]
        public float health = 1;
        public float maxHealth = 10;

        [Header("Combat")]
        public int damage = 1;
        public float moveSpeed = 1;
        public int goldReward = 0;
    }
}

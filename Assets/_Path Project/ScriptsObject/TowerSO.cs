using System;
using TDGame;
using UnityEngine;


namespace TDGame
{
    [CreateAssetMenu(fileName = "TowerSO", menuName = "Game TD/TowerSO")]
    public class TowerSO : ScriptableObject
    {
        public Sprite sprite;
        public string towerName;
        public int cost;

        [Header("Detail")]
        public TowerType towerType;
        public ProjectileType projectType;
        public Vector2Int size = new(1, 1);

        public ProjectileSO ProjectileSO;

        [Header("Combat")]
        // private float m_ShootRange;
        // private float m_ShootInterval;
        public float ShootRange;
        public float ShootInterval;

        // public float projectileSpeed;
        // public float projectileDuration;
        // public float damage;
    }
}

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
        public Vector2Int Size = new(1, 1);


        [Header("Combat")]
        public float ShootRange;
        public float ShootInterval;
        public ProjectileSO ProjectileSO;
    }
}

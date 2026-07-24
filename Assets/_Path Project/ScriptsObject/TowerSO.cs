using System;
using UnityEngine;


namespace TDGame
{
    [CreateAssetMenu(fileName = "TowerSO", menuName = "Game TD/TowerSO")]
    public class TowerSO : ScriptableObject
    {
        public Sprite sprite;
        public string towerName;

        [Header("Detail")]
        public TowerType towerType;
        public ProjectileType projectType;
        public Vector2Int Size = new(1, 1);
        public int cost;

        [Header("Combat")]
        public TowerAbility Ability;
        public ProjectileSO ProjectileSO;
    }

    [Serializable]
    public class TowerAbility
    {
        public float ShootRange;
        public float ShootInterval;
        public float PhysicDamage;
        public float MagicDamage;
    }
}

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
        public float PhysicalDamage;
        public float MagicDamage;

        public TowerAbility() { }
        public TowerAbility(TowerAbility other)
        {
            ShootRange = other.ShootRange;
            ShootInterval = other.ShootInterval;
            PhysicalDamage = other.PhysicalDamage;
            MagicDamage = other.MagicDamage;
        }

        public static TowerAbility operator +(TowerAbility a, TowerAbility b)
        {
            if (a == null) return new TowerAbility(b);
            if (b == null) return new TowerAbility(a);
            return new TowerAbility
            {
                ShootRange = a.ShootRange + b.ShootRange,
                ShootInterval = a.ShootInterval + b.ShootInterval,
                PhysicalDamage = a.PhysicalDamage + b.PhysicalDamage,
                MagicDamage = a.MagicDamage + b.MagicDamage
            };
        }
    }
}

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
        public Vector2Int Size = new(1, 1);
        public Price Price;

        [Header("Combat")]
        public TowerAbility Ability;
        public ProjectileSO ProjectileSO;

        public TowerSO NextTowerLevel;
    }

    [Serializable]
    public class Price
    {
        public int Gold;
        public int Rock;
        public int Wood;

        public Price(int gold = 0, int rock = 0, int wood = 0)
        {
            Gold = gold;
            Rock = rock;
            Wood = wood;
        }

        public static Price operator +(Price p1, Price p2)
        {
            if (p1 == null) return p2;
            if (p2 == null) return p1;

            return new Price(
                p1.Gold + p2.Gold,
                p1.Rock + p2.Rock,
                p1.Wood + p2.Wood
            );
        }

        public static Price operator -(Price p1, Price p2)
        {
            if (p1 == null) return new Price();
            if (p2 == null) return p1;

            return new Price(
                p1.Gold - p2.Gold,
                p1.Rock - p2.Rock,
                p1.Wood - p2.Wood
            );
        }
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

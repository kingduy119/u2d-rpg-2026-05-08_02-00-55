using TDGame;
using UnityEngine;


namespace TDGame
{
    [CreateAssetMenu(fileName = "TowerSO", menuName = "Game TD/TowerSO")]
    public class TowerSO : ScriptableObject
    {
        public TowerType towerType;
        public ProjectileType projectType;
        public Sprite sprite;

        [Header("Detail")]
        public string towerName;
        public int cost;
        public Vector2Int size = new(1, 1);

        [Header("Combat")]
        public float range;
        public float shootInterval;
        public float projectileSpeed;
        public float projectileDuration;
        public float damage;

    }
}

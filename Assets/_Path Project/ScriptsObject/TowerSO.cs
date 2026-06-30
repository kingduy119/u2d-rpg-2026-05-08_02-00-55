using TDGame;
using UnityEngine;


namespace TDGame
{
    [CreateAssetMenu(fileName = "TowerSO", menuName = "Game TD/TowerSO")]
    public class TowerSO : ScriptableObject
    {
        public ProjectileType projectType;
        public Sprite sprite;
        public GameObject towerPrefab;

        public string towerName;
        public int cost;

        public float range;
        public float shootInterval;
        public float projectileSpeed;
        public float projectileDuration;
        public float damage;

    }
}

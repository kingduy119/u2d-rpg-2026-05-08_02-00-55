using UnityEngine;

namespace TDGame
{
    public class FactoryManager : MonoBehaviour
    {
        public EnemyFactory EnemyFactory;
        public TowerFactory TowerFactory;
        public ProjectileFactory ProjectileFactory;


        public Enemy GetEnemy(EnemyType type) => EnemyFactory.GetObject(type);
        public Tower GetTower(TowerSO type) => TowerFactory.GetObject(type);
        public Projectile GetProjectile(ProjectileType type) => ProjectileFactory.GetObject(type);
    }
}
using UnityEngine;

namespace TDGame
{
    public class FactoryManager : MonoBehaviour
    {
        public EnemyFactory EnemyFactory;
        public TowerFactory TowerFactory;
        public ProjectileFactory ProjectileFactory;

        public TestFactory TestFactory;

        public Enemy GetEnemy(EnemyType type) => EnemyFactory.GetObject(type);

        public TowerBase GetTower(TowerType type) => TowerFactory.GetObject(type);
        public TowerBase GetTower(TowerSO type) => TestFactory.GetObject(type);

        public Projectile GetProjectile(ProjectileType type) => ProjectileFactory.GetObject(type);
    }
}
using UnityEngine;

namespace TDGame
{
    public class FactoryManager : MonoBehaviour
    {
        public EnemyFactory EnemyFactory;
        public TowerFactory TowerFactory;
        public ProjectileFactory ProjectileFactory;

        private NewTowerFactory NewTowerFactory;

        private void Awake()
        {
            NewTowerFactory = new();
        }

        public void LoadPrefabs()
        {
            NewTowerFactory.LoadPrefabs();
        }

        private void OnDestroy()
        {
            NewTowerFactory.Destroy();
        }

        public Enemy GetEnemy(EnemySO type) => EnemyFactory.GetObject(type);
        // public Tower GetTower(TowerSO type) => TowerFactory.GetObject(type);
        public Tower GetTower(TowerSO type) => NewTowerFactory.GetObject(type, transform);
        public Projectile GetProjectile(ProjectileType type) => ProjectileFactory.GetObject(type);
    }

    public class NewTowerFactory : NewFactory<TowerSO, Tower>
    {
        public NewTowerFactory()
        {
            loadKeys = new() { "Tower" };
        }

        protected override void MapGameObject(GameObject go)
        {
            Debug.Log($"MapGameObject: {go.name}");
            if (!go.TryGetComponent<Tower>(out var tower))
                return;

            if (!prefabs.ContainsKey(tower.TowerSO))
            {
                prefabs.Add(tower.TowerSO, go);
            }
        }
    }
}

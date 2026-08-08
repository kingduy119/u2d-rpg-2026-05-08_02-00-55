using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{
    public class FactoryManager
    {
        private GameManager _GameManager;
        private AddressableLoader loader = new();
        private AsyncOperationHandle<IList<GameObject>> handle = new();
        private List<string> labels = new() { "Pack_1" };

        private readonly TowerFactory TowerFactory = new();
        private readonly EnemyFactory EnemyFactory = new();
        private readonly ProjectileFactory ProjectileFactory = new();


        private GameObject factory = new("FactoryManager");

        public FactoryManager(GameManager GameManager)
        {
            _GameManager = GameManager;
            factory.transform.SetParent(GameManager.transform);

            handle = loader.LoadPrefabsAsync(labels);
            handle.Completed += OnCompeleted;
        }

        private void OnCompeleted(AsyncOperationHandle<IList<GameObject>> asyncHandle)
        {
            if (asyncHandle.Status == AsyncOperationStatus.Succeeded)
            {
                IList<GameObject> results = asyncHandle.Result;
                for (int i = 0; i < results.Count; i++)
                {
                    var go = results[i];
                    if (go.TryGetComponent<Tower>(out var tower))
                    {
                        TowerFactory.AddPrefab(tower);
                    }
                    else if (go.TryGetComponent<Enemy>(out var enemy))
                    {
                        EnemyFactory.AddPrefab(enemy);
                    }
                    else if (go.TryGetComponent<Projectile>(out var projectile))
                    {
                        ProjectileFactory.AddPrefab(projectile);
                    }
                }
            }
        }

        public Tower GetTower(TowerSO type) => TowerFactory.GetObject(type, factory.transform);
        public Enemy GetEnemy(EnemySO type) => EnemyFactory.GetObject(type, factory.transform);
        public Projectile GetProjectile(ProjectileSO type) => ProjectileFactory.GetObject(type, factory.transform);
    }
    // public class TestFactory : MonoBehaviour
    // {

    //     public EnemyFactory EnemyFactory;
    //     // public TowerFactory TowerFactory;
    //     public ProjectileFactory ProjectileFactory;



    //     public Enemy GetEnemy(EnemySO type) => EnemyFactory.GetObject(type);
    //     // public Tower GetTower(TowerSO type) => TowerFactory.GetObject(type);
    //     public Projectile GetProjectile(ProjectileType type) => ProjectileFactory.GetObject(type);

    // }


}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{
    public class FactoryManager
    {
        private readonly AddressableLoader loader = new();
        private AsyncOperationHandle<IList<GameObject>> handle;
        private List<string> labels = new() { "Pack_1" };

        private readonly TowerFactory TowerFactory = new();
        private readonly EnemyFactory EnemyFactory = new();
        private readonly ProjectileFactory ProjectileFactory = new();

        private GameObject _objectList;
        private GameObject ObjectList
        {
            get
            {
                if (_objectList == null)
                {
                    _objectList = new("FactoryManager");
                }
                return _objectList;
            }
        }

        public FactoryManager(GameManager GameManager)
        {
            handle = loader.LoadPrefabsAsync(labels);
            handle.Completed += OnCompeleted;
        }

        public Tower GetTower(TowerSO type) => TowerFactory.GetObject(type, ObjectList.transform);
        public Enemy GetEnemy(EnemySO type) => EnemyFactory.GetObject(type, ObjectList.transform);
        public Projectile GetProjectile(ProjectileSO type) => ProjectileFactory.GetObject(type, ObjectList.transform);



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

        public void ResetObjects()
        {
            if (_objectList != null) Object.Destroy(_objectList);
        }

        public void Destroy()
        {
            Addressables.Release(handle);
        }
    }
}

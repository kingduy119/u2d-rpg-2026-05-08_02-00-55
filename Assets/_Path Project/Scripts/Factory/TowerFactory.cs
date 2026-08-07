using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace TDGame
{
    // public class TowerFactory : Factory<TowerType, TowerBase> { }
    public class TowerFactory : Factory<TowerSO, Tower> { }



    // public class TowerFactoryNew : NewFactory<TowerSO, Tower>
    // {
    //     protected override void Awake()
    //     {
    //         loadKeys = new() { "Tower" };
    //         base.Awake();
    //     }

    //     protected override void OnLoadCompelete(AsyncOperationHandle<IList<GameObject>> asyncHandle)
    //     {
    //         if (asyncHandle.Status == AsyncOperationStatus.Succeeded)
    //         {
    //             IList<GameObject> results = asyncHandle.Result;
    //             for (int i = 0; i < results.Count; i++)
    //             {
    //                 MapTower(results[i]);
    //             }

    //         }
    //     }

    //     private void MapTower(GameObject go)
    //     {
    //         Debug.Log($"TowerFactoryNew: {go.name}");
    //         if (!go.TryGetComponent<Tower>(out var tower))
    //             return;

    //         if (!prefabs.ContainsKey(tower.TowerSO))
    //         {
    //             prefabs.Add(tower.TowerSO, go);
    //         }
    //     }
    // }
}
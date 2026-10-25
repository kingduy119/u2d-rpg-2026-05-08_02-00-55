using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using Characters;
using Weapons;
using VContainer.Unity;
using System;
using Cysharp.Threading.Tasks;


public class FactoryService : FactoryManager,
    IStartable,
    IDisposable
{
    public void Start()
    {
        Debug.Log("FactoryService.Start");
    }

    public void Dispose()
    {
        Debug.Log("FactoryService.Dispose");
    }

}

public class FactoryManager
{
    protected readonly AddressableLoader loader = new();
    private readonly Factory<ProjectSO, Projectile> projectiles = new("ProjectFactory");
    private readonly Factory<CharacterSO, Character> characters = new("CharacterFactory");

    public Projectile GetPrefab(ProjectSO type) => projectiles.GetObject(type);
    public Character GetPrefab(CharacterSO type) => characters.GetObject(type);

    public async UniTask LoadAssets()
    {
        string[] keys = {
            AddressableLabels.Inventory,
            AddressableLabels.Character,
            AddressableLabels.Projectile
        };

        var handle = loader.LoadAssetsAsync(keys);
        await handle.ToUniTask();
        OnCompeleted(handle);
        await UniTask.Delay(500);
    }

    protected void OnCompeleted(AsyncOperationHandle<IList<GameObject>> asyncHandle)
    {
        if (asyncHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<GameObject> results = asyncHandle.Result;
            for (int i = 0; i < results.Count; i++)
            {
                var go = results[i];
                if (go.TryGetComponent<Projectile>(out var projectile))
                {
                    projectiles.AddPrefab(projectile.SO, projectile);
                }
                else if (go.TryGetComponent<Character>(out var cter))
                {
                    characters.AddPrefab(cter.SO, cter);
                }
            }
        }
    }

    public void ClearPool()
    {
        projectiles.ClearPool();
        characters.ClearPool();
    }

    public void Release()
    {
        loader.Release();
    }
}
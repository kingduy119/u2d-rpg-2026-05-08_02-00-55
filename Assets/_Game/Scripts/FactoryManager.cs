using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using Characters;
using Weapons;
using VContainer.Unity;
using System;


public class FactoryService : FactoryManager,
    IStartable, IDisposable
{
    public FactoryService() : base() { }

    public void Start()
    {
        // throw new NotImplementedException();
        LoadAssets();
        Debug.Log("FactoryService.Start");
    }

    public void Dispose()
    {
        // throw new NotImplementedException();
        Debug.Log("FactoryService.Dispose");
    }
}

public class FactoryManager
{
    private readonly string[] keys = { "inventory_pack", "character_pack" };
    protected readonly AddressableLoader loader = new();
    private readonly Factory<ProjectSO, Projectile> projectiles = new("ProjectFactory");
    private readonly Factory<CharacterSO, Character> characters = new("CharacterFactory");

    public Projectile GetPrefab(ProjectSO type) => projectiles.GetObject(type);
    public Character GetPrefab(CharacterSO type) => characters.GetObject(type);

    public void LoadAssets()
    {
        loader.LoadAssetsAsync(keys).Completed += OnCompeleted;
    }

    protected void OnCompeleted(AsyncOperationHandle<IList<GameObject>> asyncHandle)
    {
        Debug.Log($"FactoryManager.OnCompeleted: {asyncHandle.Status}");
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

    public void Release()
    {
        loader.Release();
    }
}
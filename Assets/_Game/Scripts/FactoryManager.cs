using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using Characters;
using Weapons;

public class FactoryManager
{
    readonly AddressableLoader loader = new AddressableLoader(new[] {
            "inventory_pack",
            "character_pack"
        });
    private readonly Factory<ProjectSO, Projectile> projectiles = new("ProjectFactory");
    private readonly Factory<CharacterSO, Character> characters = new("CharacterFactory");

    public Projectile GetPrefab(ProjectSO type) => projectiles.GetObject(type);
    public Character GetPrefab(CharacterSO type) => characters.GetObject(type);

    public void LoadAssets()
    {
        loader.LoadAssetsAsync().Completed += OnCompeleted;
    }

    private void OnCompeleted(AsyncOperationHandle<IList<GameObject>> asyncHandle)
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

    public void Release()
    {
        // projectiles.Clear();
        loader.Release();
    }
}
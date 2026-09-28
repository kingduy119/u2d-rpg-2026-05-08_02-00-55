

using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Weapons;

namespace Game
{
    public class GameManagerView : MonoBehaviour
    {
        public ProjectSO projectSO;

        FactoryManager factoryGM = new();

        private void Awake()
        {
            factoryGM.LoadAssets();
        }

        private void OnEnable()
        {
            CterEvent.Shoot += Character_Shoot;
        }
        private void OnDisable()
        {
            CterEvent.Shoot -= Character_Shoot;
        }

        private void Character_Shoot(Character cter)
        {
            Vector2 direction = (cter.Target.position - cter.transform.position).normalized;
            var go = factoryGM.GetPrefab(projectSO);
            if (go) go.Launch(cter.transform, direction);
        }
    }

    public class FactoryManager
    {
        readonly AddressableLoader loader = new AddressableLoader(new[] {
            "inventory_projectile",
            "character_pack"
        });
        private readonly Factory<ProjectSO, Projectile> projectileFactory = new("ProjectFactory");
        private readonly Factory<CharacterSO, Character> facCharacter = new("CharacterFactory");

        public Projectile GetPrefab(ProjectSO type) => projectileFactory.GetObject(type);


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
                        projectileFactory.AddPrefab(projectile.SO, projectile);
                    }
                    else if (go.TryGetComponent<Character>(out var cter))
                    {
                        facCharacter.AddPrefab(cter.SO, cter);
                    }
                }
                Debug.Log($"results.Count: {results.Count}");
            }
        }

        public void Release()
        {
            // projectileFactory.Clear();
            loader.Release();
        }
    }
}
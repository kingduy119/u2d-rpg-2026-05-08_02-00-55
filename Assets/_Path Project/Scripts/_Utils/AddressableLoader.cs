using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{

    public interface IAssetLoader
    {
        Task<GameObject> LoadPrefabAsync(string key);

        Task<GameObject> InstantiateAsync(string key);

        void Release(GameObject obj);
    }

    public class AddressableLoader : IAssetLoader
    {
        public async Task<GameObject> LoadPrefabAsync(string key)
        {
            return await Addressables
                .LoadAssetAsync<GameObject>(key)
                .Task;
        }

        public AsyncOperationHandle<IList<GameObject>> LoadPrefabsAsync(
            List<string> keys,
            Addressables.MergeMode mode = Addressables.MergeMode.Union)
        {
            return Addressables.LoadAssetsAsync<GameObject>(keys, null, mode);
        }

        public async Task<GameObject> InstantiateAsync(string key)
        {
            return await Addressables
                .InstantiateAsync(key)
                .Task;
        }

        public void Release(GameObject obj)
        {
            Addressables.ReleaseInstance(obj);
        }
    }

    public class PrefabManager
    {
        private readonly List<string> labels;
        private AsyncOperationHandle<IList<GameObject>> handle;
        private AddressableLoader loader;

        public PrefabManager()
        {
            loader = new();
            labels = new() { "Pack_1" };
        }

        public void LoadPrefabs()
        {
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
                        PrefabEvent.LoadTower?.Invoke(tower);
                    }
                    else if (go.TryGetComponent<Enemy>(out var enemy))
                    {
                        Debug.Log($"enemy.name: {enemy.name}");
                    }
                    // Debug.Log($"results[i].name: {results[i].name}");
                }

            }
        }
    }
}
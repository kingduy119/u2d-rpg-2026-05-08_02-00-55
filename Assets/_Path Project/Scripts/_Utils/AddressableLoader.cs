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

}
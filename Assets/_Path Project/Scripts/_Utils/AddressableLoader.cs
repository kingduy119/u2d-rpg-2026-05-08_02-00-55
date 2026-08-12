using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{

    public interface IAssetLoader
    {
        AsyncOperationHandle<GameObject> LoadPrefabAsync(string key);
        AsyncOperationHandle<IList<GameObject>> LoadPrefabsAsync(List<string> keys, Addressables.MergeMode mode);

        Task<GameObject> InstantiateAsync(string key);

        void Release(GameObject obj);
    }

    public class AddressableLoader : IAssetLoader
    {
        public AsyncOperationHandle<GameObject> LoadPrefabAsync(string key)
        {
            return Addressables
                .LoadAssetAsync<GameObject>(key);
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
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

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
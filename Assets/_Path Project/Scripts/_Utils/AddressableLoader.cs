using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

namespace TDGame
{
    public class AssetLoader
    {
        readonly string _AssetKey;
        GameObject _gameObject;
        AsyncOperationHandle<GameObject> _handle;
        public bool IsLoaded { get; private set; }


        public AssetLoader(string key, bool autoload = false)
        {
            _AssetKey = key;
            if (autoload) LoadAsset();
        }

        public void LoadAsset()
        {
            Coroutines.StartCoroutine(LoadAssetCoroutine());
        }

        IEnumerator LoadAssetCoroutine()
        {
            _handle = Addressables.LoadAssetAsync<GameObject>(_AssetKey);
            yield return _handle;

            IsLoaded = _handle.Status == AsyncOperationStatus.Succeeded;
        }

        public GameObject Instantiate(Transform parent = null)
        {
            if (!IsLoaded) return null;

            if (_gameObject != null) return _gameObject;

            _gameObject = Object.Instantiate(_handle.Result, parent);

            return _gameObject;
        }

        public void Release()
        {
            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
                _handle = default;
            }
        }

    }


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
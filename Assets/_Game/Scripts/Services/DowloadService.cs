
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer.Unity;

public static class AddressableLabels
{
    public const string Default = "default";
    public const string Inventory = "inventory_pack";
    public const string Character = "character_pack";
    public const string Projectile = "projectile_pack";
}

public class DownloadService : IStartable,//IAsyncStartable
IDisposable
{
    long totalSize;
    long downloadedSize = 0;
    float percentComplete = 0f;
    private readonly string[] keys = {
        AddressableLabels.Default,
        AddressableLabels.Inventory,
        AddressableLabels.Character,
        // AddressableLabels.Projectile
    };

    SceneLoadingView _SceneLoadingView;
    readonly FactoryService _factoryService;

    public DownloadService(
        FactoryService factoryService
    )
    {
        _factoryService = factoryService;
    }

    public void Start()
    {
        SceneEvents.ChangeScene += SceneEvents_ChangeScene;
    }

    public void Dispose()
    {
        SceneEvents.ChangeScene -= SceneEvents_ChangeScene;
    }

    private void SceneEvents_ChangeScene(string sceneName) => LoadScene(sceneName).Forget();

    private async UniTask SyncAssets()
    {
        var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
        try
        {
            totalSize = await sizeHandle.ToUniTask();
            if (totalSize > 0)
            {
                _SceneLoadingView.UpdateProgress(percentComplete, downloadedSize, totalSize);
                // await DownloadDependencies();

                var downloadHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
                await HandleProgress(downloadHandle, "Download Failed");

                Addressables.Release(downloadHandle);
            }

        }
        finally
        {
            await _factoryService.LoadAssets();
            Addressables.Release(sizeHandle);
        }
    }

    bool HasSyncAssets;
    string loadingSceneName = "GameMenu";

    public async UniTask StartLoading(SceneLoadingView view)
    {
        _SceneLoadingView = view;

        if (!HasSyncAssets)
        {
            await SyncAssets();
            HasSyncAssets = true;
        }

        await LoadSceneByName(loadingSceneName);
    }

    public async UniTask LoadSceneByName(string sceneName)
    {
        var loadSceneHandle = Addressables.LoadSceneAsync(
            sceneName,
            UnityEngine.SceneManagement.LoadSceneMode.Single,
            activateOnLoad: false
        );
        await HandleProgress(loadSceneHandle, "Load Scene Failed");

        if (loadSceneHandle.Status == AsyncOperationStatus.Succeeded)
        {
            await UniTask.Delay(500);
            await loadSceneHandle.Result.ActivateAsync();
        }

        Addressables.Release(loadSceneHandle);
    }


    public async UniTask LoadScene(string sceneName)
    {
        loadingSceneName = sceneName;
        await NavToLoadingScene();
    }

    private async UniTask NavToLoadingScene()
    {
        var scene = Addressables.LoadSceneAsync(
            "GameLoading",
            UnityEngine.SceneManagement.LoadSceneMode.Single,
            activateOnLoad: true
        );
        await scene.ToUniTask();
        Addressables.Release(scene);
    }

    private async UniTask HandleProgress(AsyncOperationHandle handle, string message)
    {
        while (!handle.IsDone)
        {
            var status = handle.GetDownloadStatus();
            downloadedSize = status.DownloadedBytes;
            percentComplete = status.Percent;
            _SceneLoadingView.UpdateProgress(percentComplete, downloadedSize, totalSize);
            await UniTask.Delay(500);
        }

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log(message);
        }
        else
        {
            var finalStatus = handle.GetDownloadStatus();
            downloadedSize = finalStatus.DownloadedBytes;
            percentComplete = 1f;
            _SceneLoadingView.UpdateProgress(percentComplete, downloadedSize, totalSize);
        }

        await UniTask.Delay(500);
    }


}
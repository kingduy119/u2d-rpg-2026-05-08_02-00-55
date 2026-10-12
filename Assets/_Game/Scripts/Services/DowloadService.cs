
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer.Unity;

public class DownloadService : IAsyncStartable
{
    long totalSize;
    long downloadedSize = 0;
    float percentComplete = 0f;
    private readonly string[] keys = {
        AddressableLabels.Default,
        AddressableLabels.Inventory
    };

    readonly SceneLoadingView _SceneLoadingView;
    readonly FactoryService _factoryService;

    public DownloadService(SceneLoadingView sceneLoadingView, FactoryService factoryService)
    {
        _SceneLoadingView = sceneLoadingView;
        _factoryService = factoryService;
    }

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        await SyncAssets();
        await _factoryService.LoadAssets();

        await LoadSceneByName("GameMenu");
    }
    private async UniTask SyncAssets()
    {
        var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
        try
        {
            totalSize = await sizeHandle.ToUniTask();
            if (totalSize > 0)
            {
                _SceneLoadingView.UpdateProgress(percentComplete, downloadedSize, totalSize);
                await DownloadDependencies();
            }

        }
        finally
        {
            Addressables.Release(sizeHandle);
        }
    }

    public async UniTask LoadSceneByName(string sceneName)
    {
        var loadSceneHandle = Addressables.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        await HandleProgress(loadSceneHandle, "Load Scene Failed");
    }

    private async UniTask DownloadDependencies()
    {
        var downloadHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
        await HandleProgress(downloadHandle, "Download Failed");
    }

    private async UniTask HandleProgress(AsyncOperationHandle handle, string message)
    {
        _SceneLoadingView.gameObject.SetActive(true);

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
            _SceneLoadingView.gameObject.SetActive(false);
        }

        Addressables.Release(handle);
        await UniTask.Delay(500);
    }

}
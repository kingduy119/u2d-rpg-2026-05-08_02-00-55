using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
    }

    private void Start()
    {
        Debug.Log("GameLifetimeScope Start");
        LoadSceneAsyncs().Forget();
    }

    async UniTask LoadSceneAsyncs()
    {
        var keys = new[] { "inventory_pack" };
        var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
        long totalSize;
        try
        {
            totalSize = await sizeHandle.ToUniTask();
        }
        finally
        {
            Addressables.Release(sizeHandle);
        }


        Debug.Log($"Total download: {totalSize / 1024f / 1024f:F2} MB");

        if (totalSize > 0)
        {
            Debug.Log($"Downloading dependencies... {totalSize / 1024f / 1024f:F2} MB");
            // var downloadHandle = Addressables.DownloadDependenciesAsync(keys);
            var downloadHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);

            while (!downloadHandle.IsDone)
            {
                Debug.Log($"Download progress: {downloadHandle.PercentComplete * 100f:F2}%");
                await UniTask.Delay(100);
            }

            if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Download Failed");
            }
            else
            {
                Debug.Log("Download Completed");
            }

            Addressables.Release(downloadHandle);
        }

        Debug.Log("Loading Gameplay scene...");

    }
}



using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class SceneLoadingView : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TMP_Text _progressText;

    private readonly string[] keys = {
        "default"
        ,"inventory_pack"
        // , "character_pack" 
        };
    long totalSize;
    long downloadedSize = 0;
    float percentComplete = 0f;

    private void Start()
    {
        LoadSceneAsyncs().Forget();
    }

    async UniTask LoadSceneAsyncs()
    {
        // Caching.ClearCache();
        var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
        try
        {
            totalSize = await sizeHandle.ToUniTask();
        }
        finally
        {
            Addressables.Release(sizeHandle);
        }

        if (totalSize > 0)
        {
            DownloadDependencies().Forget();
        }
        // else
        // {
        //     // Debug.Log("LoadGameplayScene ()");
        //     LoadGameplayScene().Forget();
        // }


        Debug.Log("Loading Gameplay scene...");
        LoadGameplayScene().Forget();
    }

    async UniTask LoadGameplayScene()
    {
        var loadHandle = Addressables.LoadSceneAsync("GameLifetimeScope", UnityEngine.SceneManagement.LoadSceneMode.Single);

        while (!loadHandle.IsDone)
        {
            var status = loadHandle.GetDownloadStatus();
            downloadedSize = status.DownloadedBytes;
            percentComplete = status.Percent;
            UpdateProgress();

            Debug.Log($"Loading progress: {status.Percent * 100f:F2}% " +
                      $"({SizeFormatter.Format(status.DownloadedBytes)} / {SizeFormatter.Format(status.TotalBytes)})");

            await UniTask.Delay(500);
        }

        await loadHandle.ToUniTask();
        Addressables.Release(loadHandle);
    }

    async UniTask DownloadDependencies()
    {
        var downloadHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
        while (!downloadHandle.IsDone)
        {
            var status = downloadHandle.GetDownloadStatus();
            downloadedSize = status.DownloadedBytes;
            percentComplete = status.Percent;
            UpdateProgress();

            Debug.Log($"Download progress: {status.Percent * 100f:F2}% " +
                      $"({SizeFormatter.Format(status.DownloadedBytes)} / {SizeFormatter.Format(status.TotalBytes)})");

            await UniTask.Delay(500);
        }



        if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Download Failed");
        }
        else
        {
            var finalStatus = downloadHandle.GetDownloadStatus();
            downloadedSize = finalStatus.DownloadedBytes;
            percentComplete = 1f;
            UpdateProgress();
            Debug.Log($"Download Completed: {finalStatus.Percent * 100f:F2}% " +
                      $"({SizeFormatter.Format(finalStatus.DownloadedBytes)} / {SizeFormatter.Format(finalStatus.TotalBytes)})");
        }

        Addressables.Release(downloadHandle);
    }

    private void UpdateProgress()
    {
        _progressSlider.value = percentComplete;
        _progressText.text = $"{SizeFormatter.Format(downloadedSize)} / {SizeFormatter.Format(totalSize)}";
    }
}
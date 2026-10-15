using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

public class GameManager : IStartable, IDisposable
{

    DownloadService _DownloadService;
    public GameManager(
        DownloadService downloadService
    )
    {
        _DownloadService = downloadService;
    }

    public void Start()
    {
        Debug.Log("GameManager.Start");
        _DownloadService.LoadScene("GameMenu").Forget();
    }


    public void Dispose()
    {
        Debug.Log("GameManager.Dispose");
    }
}
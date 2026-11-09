using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class GameManager : IAsyncStartable, IDisposable
{

    [Inject] DownloadService _DownloadService;
    [Inject] AuthService _AuthService;
    [Inject] FirebaseService _FirebaseService;

    // public void Start()
    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        // ButtonEvents.Login += Login;

        _AuthService.OnLoginSuccess += OnLoginSuccess;

        // AuthenticateAndLoadScene().Forget();
        // _DownloadService.LoadScene(SceneEvents.GAME_AUTH).Forget();
        _AuthService.SignInAnonymouslyAsync(cancellation).Forget();
    }

    public void Dispose()
    {
        _AuthService.OnLoginSuccess -= OnLoginSuccess;

        ButtonEvents.Login -= Login;
    }

    private void OnLoginSuccess(string PlayerId)
    {
        // Debug.Log($"OnLoginSuccess: {PlayerId}");
        // string id = await _FirebaseService.FetchUserById(PlayerId);
        // Debug.Log($"OnLoginSuccess: {id}");

        // _DownloadService.LoadScene(SceneEvents.GAME_MENU).Forget();
        LoadUserProfile(PlayerId).Forget();
    }
    private async UniTaskVoid LoadUserProfile(string PlayerId)
    {
        Debug.Log($"OnLoginSuccess: {PlayerId}");
        string id = await _FirebaseService.FetchUserById(PlayerId);
        Debug.Log($"OnLoginSuccess: {id}");

        _DownloadService.LoadScene(SceneEvents.GAME_MENU);
    }

    public void Login(LoginMethods method)
    {
        if (method == LoginMethods.Anonymously)
            _AuthService.SignInAnonymouslyAsync().Forget();
        else if (method == LoginMethods.Unity)
            _AuthService.SignOut();
    }
}


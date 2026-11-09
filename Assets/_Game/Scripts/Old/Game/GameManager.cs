using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class GameManager : IAsyncStartable, IDisposable
{

    [Inject] readonly DownloadService _DownloadService;
    [Inject] readonly AuthService _AuthService;
    [Inject] readonly FirebaseService _FirebaseService;
    [Inject] readonly FacebookService _FacebookService;

    // public void Start()
    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        ButtonEvents.OnAuth += OnAuth;

        _AuthService.OnLoginSuccess += OnLoginSuccess;

        _AuthService.SignInAnonymouslyAsync(cancellation).Forget();
    }

    public void Dispose()
    {
        _AuthService.OnLoginSuccess -= OnLoginSuccess;
        ButtonEvents.OnAuth -= OnAuth;
    }

    private void OnLoginSuccess(string PlayerId) => LoadUserProfileAsync(PlayerId).Forget();
    private async UniTaskVoid LoadUserProfileAsync(string PlayerId)
    {
        string id = await _FirebaseService.FetchUserById(PlayerId);
        Debug.Log($"OnLoginSuccess: {id}");

        // await _DownloadService.LoadScene(SceneEvents.GAME_MENU);
        await _DownloadService.LoadScene(SceneEvents.GAME_AUTH);
    }

    public void OnAuth(AuthMethods method)
    {
        switch (method)
        {
            case AuthMethods.Anonymously:
                _AuthService.SignInAnonymouslyAsync().Forget();
                break;
            case AuthMethods.Facebook:
                _FacebookService.Login();
                break;
            case AuthMethods.FacebookUnlink:
                _FacebookService.UnlinkFacebookAsync().Forget();
                break;
            case AuthMethods.Logout:
                _AuthService.SignOut();
                break;
            default:
                break;
        }
    }
}


using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;
using System.Threading;
using UnityEngine;

public class GameManager : IAsyncStartable, IDisposable
{

    // [Inject] readonly DownloadService _DownloadService;
    [Inject] readonly AuthService _AuthService;
    // [Inject] readonly FirebaseService _FirebaseService;
    [Inject] readonly FacebookService _FacebookService;
    [Inject] readonly ProfileSerice _ProfileService;
    [Inject] readonly FactoryService _FactoryService;

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        ButtonEvents.OnAuth += OnAuth;

        _AuthService.OnLoginSuccess += OnLoginSuccess;
        _AuthService.SignInAnonymouslyAsync(cancellation).Forget();
    }

    public void Dispose()
    {
        ButtonEvents.OnAuth -= OnAuth;

        _AuthService.OnLoginSuccess -= OnLoginSuccess;

        _FactoryService.Release();

    }

    private void OnLoginSuccess(string PlayerId) =>
        _ProfileService.LoadUserAsync(PlayerId).Forget();

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


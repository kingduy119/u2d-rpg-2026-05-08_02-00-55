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
    [Inject] readonly ProfileSerice _ProfileService;

    public UserData User;
    public ResourceData UserResource;

    // public void Start()
    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        ButtonEvents.OnAuth += OnAuth;
        ButtonEvents.ConfirmChangeNickname += ConfirmChangeNickname;

        _AuthService.OnLoginSuccess += OnLoginSuccess;

        _AuthService.SignInAnonymouslyAsync(cancellation).Forget();
    }

    public void Dispose()
    {
        _AuthService.OnLoginSuccess -= OnLoginSuccess;
        ButtonEvents.OnAuth -= OnAuth;
        ButtonEvents.ConfirmChangeNickname -= ConfirmChangeNickname;
    }

    private void OnLoginSuccess(string PlayerId) => LoadUserProfileAsync(PlayerId).Forget();
    private async UniTaskVoid LoadUserProfileAsync(string PlayerId)
    {
        var User = await _FirebaseService.GetUserAsync(PlayerId);

        if (User == null)
        {
            User = new()
            {
                UserId = PlayerId,
                Name = "New Player",
            };
            UserResource = new();
            await _FirebaseService.SaveUserAsync(User);
            await _FirebaseService.SaveUserResourceAsync(User, UserResource);
        }
        else
        {
            UserResource = await _FirebaseService.LoadUserResourceAsync(User);
        }

        var scene = User.FirstLogin ? SceneEvents.GAME_AUTH : SceneEvents.GAME_MENU;
        await _DownloadService.LoadScene(scene);
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

    private void ConfirmChangeNickname(string nickname)
    {
        if (User != null)
        {
            User.Nickname = nickname;
            User.FirstLogin = false;
            _FirebaseService.SaveUserAsync(User, onSuccess: () =>
            {
                _DownloadService.LoadScene(SceneEvents.GAME_MENU).Forget();
            });
        }
        else
        {
            Debug.LogWarning("User is null. Cannot set nickname.");
        }
    }
}

public class UserData
{
    public string UserId;
    public string Name;
    public string Nickname;
    public int Level;
    public bool FirstLogin = true;
}

public class ResourceData
{
    public int Diamond;
    public int Gold;
}
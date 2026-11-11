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

    // public void Start()
    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        ButtonEvents.OnAuth += OnAuth;
        ButtonEvents.ChangeNickname += ChangeNickname;

        _AuthService.OnLoginSuccess += OnLoginSuccess;

        _AuthService.SignInAnonymouslyAsync(cancellation).Forget();
    }

    public void Dispose()
    {
        _AuthService.OnLoginSuccess -= OnLoginSuccess;
        ButtonEvents.OnAuth -= OnAuth;
        ButtonEvents.ChangeNickname -= ChangeNickname;
    }

    private void OnLoginSuccess(string PlayerId) => LoadUserProfileAsync(PlayerId).Forget();
    private async UniTaskVoid LoadUserProfileAsync(string PlayerId)
    {
        var user = await _FirebaseService.GetUserAsync(PlayerId);
        Debug.Log($"OnLoginSuccess: {user != null}");

        if (user == null)
        {
            user = new UserData()
            {
                UserId = PlayerId,
                Name = "New Player",
            };
            _FirebaseService.SaveUserAsync(user);
        }

        User = user;
        Debug.Log($"_FirebaseService.SetUserAsync: {user != null}");

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

    private void ChangeNickname(string nickname)
    {
        if (User != null)
        {
            User.Nickname = nickname;
            Debug.Log($"SetNickname: {nickname}");
            _FirebaseService.SaveUserAsync(User);
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
}

public class ResourceData
{
    public int Diamond;
    public int Gold;
}
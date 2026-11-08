using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public class GameManager : IStartable, IDisposable
{

    readonly DownloadService _DownloadService;
    readonly AuthService _AuthService;

    [Inject]
    readonly FirebaseService _FirebaseService;

    public GameManager(DownloadService downloadService, AuthService auth)
    {
        _DownloadService = downloadService;
        _AuthService = auth;
    }

    public void Start()
    {
        ButtonEvents.Login += Login;

        _AuthService.OnLoginSuccess += _FirebaseService.FetchUserById;

        // _DownloadService.LoadScene(SceneEvents.GAME_MENU).Forget();
        _DownloadService.LoadScene(SceneEvents.GAME_AUTH).Forget();
    }


    public void Dispose()
    {
        _AuthService.OnLoginSuccess -= _FirebaseService.FetchUserById;

        ButtonEvents.Login -= Login;
    }

    public void Login(LoginMethods method)
    {
        if (method == LoginMethods.Anonymously)
            _AuthService.SignInAnonymouslyAsync().Forget();
        else if (method == LoginMethods.Unity)
            _AuthService.StartPlayerAccountsSignInAsync().Forget();
    }
}


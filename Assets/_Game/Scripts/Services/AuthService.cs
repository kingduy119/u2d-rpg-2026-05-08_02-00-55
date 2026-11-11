using System;
using System.Threading;
using VContainer.Unity;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Cysharp.Threading.Tasks;
using Facebook.Unity;
using System.Collections.Generic;


public class AuthService : IAsyncStartable, IDisposable //IAsyncStartable, IStartable
{
    private CancellationToken _token = default;
    public event Action<string> OnLoginSuccess;

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        _token = cancellation;

        await UnityServices.InitializeAsync().AsUniTask().AttachExternalCancellation(cancellation);

        // PlayerAccountService.Instance.SignedIn += OnPlayerAccountSignedIn;

        RegistEvents();
    }
    public void RegistEvents()
    {
        // PlayerAccountService.Instance.SignedIn += OnPlayerAccountSignedIn;
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
          {
              Debug.Log("Player session could not be refreshed and expired.");
          };
    }

    public void Dispose()
    {
        PlayerAccountService.Instance.SignedIn -= OnPlayerAccountSignedIn;
    }

    private void OnPlayerAccountSignedIn()
    {
        Debug.Log("OnPlayerAccountSignedIn");
        SignInWithUnityAuthAsync(_token).Forget();
    }

    public async UniTask SignInAnonymouslyAsync(CancellationToken cancellation = default)
    {
        // 1. Khởi tạo UGS (chỉ làm 1 lần)
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await UnityServices.InitializeAsync().AsUniTask().AttachExternalCancellation(cancellation);
        }
        // Nếu đang Initializing (do chỗ khác gọi), chờ cho xong
        else if (UnityServices.State == ServicesInitializationState.Initializing)
        {
            await UniTask.WaitUntil(
                () => UnityServices.State != ServicesInitializationState.Initializing,
                cancellationToken: cancellation);
        }

        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync()
                    .AsUniTask()
                    .AttachExternalCancellation(cancellation);
            }
            OnLoginSuccess?.Invoke(AuthenticationService.Instance.PlayerId);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex); // So sánh ex.ErrorCode với AuthenticationErrorCodes
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex); // So sánh ex.ErrorCode với CommonErrorCodes
        }
    }

    private async UniTask SignInWithUnityAuthAsync(CancellationToken cancellation = default)
    {
        try
        {
            await AuthenticationService.Instance
                .SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken)
                .AsUniTask()
                .AttachExternalCancellation(cancellation);

            OnLoginSuccess?.Invoke(AuthenticationService.Instance.PlayerId);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex); // So sánh ex.ErrorCode với AuthenticationErrorCodes
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex); // So sánh ex.ErrorCode với CommonErrorCodes
        }
    }

    public void SignOut()
    {
        // Với anonymous, tham số true sẽ xóa session đã lưu
        // => lần sau sẽ thành tài khoản mới. Dùng false để chỉ đăng xuất tạm.
        AuthenticationService.Instance.SignOut(false);
    }

} // class AuthService

public class FacebookService : IStartable
{
    public string Token;
    public string Error;

    public void Start()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0; // Pause the game - we will need to hide
        }
        else
        {
            Time.timeScale = 1; // Resume the game - we're getting focus again
        }
    }

    public void Login()
    {
        Debug.Log($"Facebook.Login");
        // Define the permissions
        var perms = new List<string>() { "public_profile", "email" };

        FB.LogInWithReadPermissions(perms, async result =>
        {
            if (FB.IsLoggedIn)
            {
                Token = AccessToken.CurrentAccessToken.TokenString;
                Debug.Log($"Facebook Login token: {Token}");

                if (AuthenticationService.Instance.IsSignedIn)
                    await LinkWithFacebookAsync(Token);
                else
                    await SignInWithFacebookAsync(Token);
            }
            else
            {
                Error = "User cancelled login";
                Debug.Log("[Facebook Login] User cancelled login");
            }
        });
    }

    private async UniTask LinkWithFacebookAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithFacebookAsync(accessToken);
            Debug.Log("Successfully linked with Facebook!");

        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            Debug.Log("Account is already linked with another Facebook account");
        }
        catch (Exception ex)
        {
            Debug.Log($"Facebook linking failed: {ex.Message}");
        }
    }

    public async UniTask UnlinkFacebookAsync()
    {
        try
        {
            await AuthenticationService.Instance.UnlinkFacebookAsync();
            Debug.Log("Unlink is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            Debug.LogException(ex);
        }
    }


    async UniTask SignInWithFacebookAsync(string token)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(token);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
}
using System;
using System.Threading;
using VContainer.Unity;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Cysharp.Threading.Tasks;


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

        // Đã đăng nhập (session còn từ lần trước) thì không cần làm lại
        if (AuthenticationService.Instance.IsSignedIn)
            return;

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync()
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

    // if (AuthenticationService.Instance.IsSignedIn)
    //     return true;

    // Không có session lưu: đã logout hoặc mới cài game
    // if (!AuthenticationService.Instance.SessionTokenExists)
    // {
    //     Debug.Log("Chưa đăng nhập, cần hiện màn hình Login.");
    //     return false;
    // }

    // public async UniTask TryRestoreSessionAsync(CancellationToken cancellation = default)
    // {
    //     try
    //     {
    //         await UnityServices.InitializeAsync().AsUniTask().AttachExternalCancellation(cancellation);

    //         if (!AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.SessionTokenExists)
    //         {
    //             // Có token lưu sẵn: gọi lại sẽ khôi phục đúng tài khoản cũ
    //             await AuthenticationService.Instance.SignInAnonymouslyAsync()
    //                             .AsUniTask()
    //                             .AttachExternalCancellation(cancellation);
    //         }
    //         else
    //         {
    //             await AuthenticationService.Instance
    //             .SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken)
    //             .AsUniTask()
    //             .AttachExternalCancellation(cancellation);
    //         }


    //         OnLoginSuccess?.Invoke(AuthenticationService.Instance.PlayerId);
    //     }
    //     catch (OperationCanceledException)
    //     {
    //         throw;
    //     }
    //     catch (Exception ex) when (ex is AuthenticationException || ex is RequestFailedException)
    //     {
    //         Debug.LogException(ex);
    //     }
    // }

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

    // Login with account unity
    // public async UniTask StartPlayerAccountsSignInAsync(CancellationToken cancellation = default)
    // {
    //     if (PlayerAccountService.Instance.IsSignedIn)
    //     {
    //         // Đã đăng nhập Player Accounts, chuyển thẳng sang Unity Authentication
    //         await SignInWithUnityAuthAsync(cancellation);
    //         return;
    //     }

    //     try
    //     {
    //         // Mở trình duyệt hệ thống để đăng nhập Unity Player Accounts
    //         await PlayerAccountService.Instance.StartSignInAsync()
    //             .AsUniTask()
    //             .AttachExternalCancellation(cancellation);
    //     }
    //     catch (OperationCanceledException)
    //     {
    //         throw;
    //     }
    //     catch (PlayerAccountsException ex)
    //     {
    //         // So sánh ex.ErrorCode với PlayerAccountsErrorCodes
    //         Debug.LogException(ex);
    //     }
    //     catch (RequestFailedException ex)
    //     {
    //         // So sánh ex.ErrorCode với CommonErrorCodes
    //         Debug.LogException(ex);
    //     }
    // }





    public void SignOut()
    {
        // Với anonymous, tham số true sẽ xóa session đã lưu
        // => lần sau sẽ thành tài khoản mới. Dùng false để chỉ đăng xuất tạm.
        AuthenticationService.Instance.SignOut(false);
    }

} // class AuthService
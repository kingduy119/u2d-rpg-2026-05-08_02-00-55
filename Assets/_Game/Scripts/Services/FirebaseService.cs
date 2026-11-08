
using VContainer.Unity;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using System;


public class FirebaseService : IStartable
{
    public DatabaseReference db;
    // FirebaseApp app;

    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // app = FirebaseApp.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance.RootReference;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void WriteDatabase(string name, string key, string value)
    {
        db.Child(name).Child(key).SetValueAsync(value).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data written successfully.");
            }
            else
            {
                Debug.LogError("Failed to write data: " + task.Exception);
            }
        });
    }

    public void ReadDatabase(string name, string key)
    {
        db.Child(name).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log("Data read successfully: " + snapshot.Value.ToString());
                }
                else
                {
                    Debug.LogWarning("No data found at the specified path.");
                }
            }
            else
            {
                Debug.LogError("Failed to read data: " + task.Exception);
            }
        });
    }

    public void FetchUserById(string id)
    {
        Debug.Log($"FirebaseService.FetchUserById: {id}");
    }

    public async void TestSaveData()
    {
        var player = new PlayerData
        {
            Name = "Player01",
            Level = 10,
            Gold = 500,
            Characters = new List<CharacterData>
            {
                new() {
                    Id = "Archer",
                    Level = 5,
                    Star = 2
                },
                new() {
                    Id = "Knight",
                    Level = 3,
                    Star = 1
                }
            }
        };

        string json = JsonUtility.ToJson(player);
        await db.Child("Players")
                .Child("Player01")
                .SetRawJsonValueAsync(json);

        Debug.Log("Save success!");
    }
}

[System.Serializable]
public class PlayerData
{
    public string Name;
    public int Level;
    public int Gold;
    public List<CharacterData> Characters;
}

[System.Serializable]
public class CharacterData
{
    public string Id;
    public int Level;
    public int Star;
}




public class AuthService : IAsyncStartable, IDisposable
{
    private CancellationToken _token;
    public event Action<string> OnLoginSuccess;

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        _token = cancellation;

        await UnityServices.InitializeAsync().AsUniTask().AttachExternalCancellation(cancellation);

        PlayerAccountService.Instance.SignedIn += OnPlayerAccountSignedIn;
    }

    public void Dispose()
    {
        PlayerAccountService.Instance.SignedIn -= OnPlayerAccountSignedIn;
    }

    private void OnPlayerAccountSignedIn()
    {
        SignInWithUnityAuthAsync(_token).Forget();
    }

    private async UniTask SignInWithUnityAuthAsync(
        // Action<string> OnSuccess = null,
        CancellationToken cancellation = default)
    {
        try
        {
            await AuthenticationService.Instance
                .SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken)
                .AsUniTask()
                .AttachExternalCancellation(cancellation);

            Debug.Log("SignIn is successful.");
            OnLoginSuccess?.Invoke(AuthenticationService.Instance.PlayerId);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (AuthenticationException ex)
        {
            // So sánh ex.ErrorCode với AuthenticationErrorCodes
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // So sánh ex.ErrorCode với CommonErrorCodes
            Debug.LogException(ex);
        }
    }

    // Login with account unity
    public async UniTask StartPlayerAccountsSignInAsync(CancellationToken cancellation = default)
    {
        if (PlayerAccountService.Instance.IsSignedIn)
        {
            // Đã đăng nhập Player Accounts, chuyển thẳng sang Unity Authentication
            await SignInWithUnityAuthAsync(cancellation);
            return;
        }

        try
        {
            // Mở trình duyệt hệ thống để đăng nhập Unity Player Accounts
            await PlayerAccountService.Instance.StartSignInAsync()
                .AsUniTask()
                .AttachExternalCancellation(cancellation);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (PlayerAccountsException ex)
        {
            // So sánh ex.ErrorCode với PlayerAccountsErrorCodes
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // So sánh ex.ErrorCode với CommonErrorCodes
            Debug.LogException(ex);
        }
    }

    public async UniTask SignInAnonymouslyAsync(
        // Action<string> OnSuccess = null,
        CancellationToken cancellation = default)
    {
        // Đã đăng nhập (session còn từ lần trước) thì không cần làm lại
        if (AuthenticationService.Instance.IsSignedIn)
            return;

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync()
                .AsUniTask()
                .AttachExternalCancellation(cancellation);

            Debug.Log($"Anonymous sign-in OK. PlayerId: {AuthenticationService.Instance.PlayerId}");
            OnLoginSuccess?.Invoke(AuthenticationService.Instance.PlayerId);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (AuthenticationException ex)
        {
            // So sánh ex.ErrorCode với AuthenticationErrorCodes
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // So sánh ex.ErrorCode với CommonErrorCodes
            Debug.LogException(ex);
        }
    }

    public void SignOut()
    {
        // Với anonymous, tham số true sẽ xóa session đã lưu
        // => lần sau sẽ thành tài khoản mới. Dùng false để chỉ đăng xuất tạm.
        AuthenticationService.Instance.SignOut(false);
    }


}



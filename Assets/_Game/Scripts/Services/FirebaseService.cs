
using VContainer.Unity;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;


public class FirebaseService : IStartable
{
    public DatabaseReference db;

    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                db = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
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

    private async UniTask<bool> Run(Func<UniTask> action, string name, Action onSuccess = default)
    {
        try
        {
            await action();
        }
        catch (OperationCanceledException)
        {
            return false; // bị hủy chủ động, không cần log lỗi
        }
        catch (Exception e)
        {
            Debug.LogError($"{name} Error: {e.Message}");
            return false;
        }
        onSuccess?.Invoke();
        return true;

    }

    private async UniTask<T> Run<T>(Func<UniTask<T>> action, string name, T fallback = default)
    {
        try
        {
            return await action();
        }
        catch (OperationCanceledException)
        {
            Debug.LogError($"{name} OperationCanceledException");
            return fallback;
        }
        catch (Exception e)
        {
            Debug.LogError($"{name} Error: {e.Message}");
            return fallback;
        }
    }

    public async UniTask<string> FetchUserById(string id)
    {
        ReadDatabase("Users", id);
        return id;
    }

    public async UniTask<UserData> GetUserAsync(string playerId)
    {
        DataSnapshot snap = await db.Child("users").Child(playerId).GetValueAsync();
        if (!snap.Exists) return null;
        return JsonUtility.FromJson<UserData>(snap.GetRawJsonValue());
    }

    public UniTask<bool> SaveUserAsync(UserData data, Action onSuccess = null) =>
        Run(() => db.Child("users").Child(data.UserId)
            .SetRawJsonValueAsync(JsonUtility.ToJson(data))
            .AsUniTask(),
        "SaveUser",
        onSuccess);

    public UniTask<UserData> LoadUserAsync(string userId) =>
    Run(async () =>
    {
        var snap = await db.Child("users").Child(userId).GetValueAsync().AsUniTask();
        return snap.Exists
            ? JsonUtility.FromJson<UserData>(snap.GetRawJsonValue())
            : null;
    }, "LoadUser");

    public UniTask<bool> SaveUserResourceAsync(UserData user, ResourceData resource, Action onSuccess = null) =>
        Run(() => db.Child("user-resources").Child(user.UserId)
                .SetRawJsonValueAsync(JsonUtility.ToJson(resource))
                .AsUniTask(),
            "SaveUserResource",
            onSuccess);

    public UniTask<ResourceData> LoadUserResourceAsync(UserData user) =>
        Run(async () =>
        {
            var snap = await db.Child("user-resources").Child(user.UserId).GetValueAsync().AsUniTask();
            return snap.Exists
                ? JsonUtility.FromJson<ResourceData>(snap.GetRawJsonValue())
                : null;
        }, "LoadUserResource");
}



using VContainer.Unity;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;


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

    public async UniTask<string> FetchUserById(string id)
    {
        // await UniTask.Delay(1000); // Simulate an asynchronous operation
        ReadDatabase("Users", id);
        return id;
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








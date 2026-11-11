using System;
using UnityEngine;
using VContainer.Unity;
using VContainer;

public abstract class Service : IStartable, IDisposable
{
    public virtual void Start()
    {
        Debug.Log($"{GetType().Name}.Start");
    }
    public virtual void Dispose()
    {
        Debug.Log($"{GetType().Name}.Dispose");
    }
}

public class ProfileSerice : Service
{
    public Resources gResources { get; private set; } = new();
    // public UserData User;

    // public override void Start()
    // {
    //     Debug.Log("ProfileSerice.Start");
    //     ButtonEvents.ChangeNickname += SetNickname;
    // }

    // public override void Dispose()
    // {
    //     Debug.Log("ProfileSerice.Dispose");
    //     ButtonEvents.ChangeNickname -= SetNickname;
    // }

    // private void SetNickname(string nickname)
    // {
    //     if (User != null)
    //     {
    //         User.Nickname = nickname;
    //         Debug.Log($"SetNickname: {nickname}");
    //     }
    //     else
    //     {
    //         Debug.LogWarning("User is null. Cannot set nickname.");
    //     }
    // }
}



using System;
using UnityEngine;
using VContainer.Unity;

public class ProfileSerice : IStartable
{
    public Resources gResources { get; private set; } = new();

    public void Start()
    {
        Debug.Log("ProfileSerice.Start");
    }
}


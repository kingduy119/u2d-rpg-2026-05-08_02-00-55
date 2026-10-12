using System;
using UnityEngine;
using VContainer.Unity;

public class GameManager : IStartable, IDisposable
{

    public GameManager()
    {
        Debug.Log("GameManager");
    }

    public void Start()
    {
        Debug.Log("GameManager.Start");
    }


    public void Dispose()
    {
        Debug.Log("GameManager.Dispose");
    }
}
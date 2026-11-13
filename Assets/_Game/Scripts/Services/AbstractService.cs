using System;
using UnityEngine;
using VContainer.Unity;

public abstract class Service : IStartable, IDisposable, IPostStartable
{
    protected bool debug;
    public virtual void Start()
    {
        if (debug) Debug.Log($"{GetType().Name}.Start");
    }
    public virtual void Dispose()
    {
        if (debug) Debug.Log($"{GetType().Name}.Dispose");
    }

    public virtual void PostStart()
    {
        if (debug) Debug.Log($"{GetType().Name}.PostStart");
    }
}
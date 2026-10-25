using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameMenuLS : LifetimeScope
{

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameMenuController>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<CharacterDetailTabView>();
    }
}


public class GameMenuController : IDisposable
{
    readonly FactoryService _factory;

    public GameMenuController(FactoryService factory)
    {
        _factory = factory;
    }

    public void Dispose()
    {
        _factory.ClearPool();
    }
}
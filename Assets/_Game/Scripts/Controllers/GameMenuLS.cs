using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameMenuLS : LifetimeScope
{

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameMenuController>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<UserProfileView>();
        builder.RegisterComponentInHierarchy<CharacterDetailTabView>();
    }
}


public class GameMenuController : Service
{
    [Inject] readonly FactoryService _factory;
    [Inject] readonly GameManager _GameManager;

    [Inject] readonly UserProfileView _UserProfileView;


    // public GameMenuController(FactoryService factory)
    // {
    //     _factory = factory;
    // }
    public override void PostStart()
    {
        _UserProfileView.UpdateView(_GameManager.User);
    }

    public override void Dispose()
    {
        _factory.ClearPool();
    }
}
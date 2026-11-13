using System;
using System.Collections.Generic;
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

    public override void Start()
    {
        // ButtonEvents.ClaimRewards += ClaimRewards;
    }

    public override void PostStart()
    {
        _UserProfileView.UpdateView(_GameManager.User);
    }

    public override void Dispose()
    {
        _factory.ClearPool();
    }

    // private void ClaimRewards(List<ItemSO> rewards)
    // {
    //     foreach (var reward in rewards)
    //     {
    //         switch (reward.Type)
    //         {
    //             case ItemType.Diamond:
    //                 _GameManager.UserResource.Diamond += reward.value;
    //                 break;
    //             case ItemType.Gold:
    //                 _GameManager.UserResource.Gold += reward.value;
    //                 break;
    //             default: break;
    //         }
    //     }

    //     _UserProfileView.UpdateView(_GameManager.UserResource);
    // }
}
using System.Collections.Generic;
using AdsLib;
using Core;
using Cysharp.Threading.Tasks;
using IAPLib;
using InventoryLib;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameMenuLS : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<MainMenuController>(Lifetime.Singleton);
        builder.RegisterEntryPoint<IAPController>(Lifetime.Singleton);

        builder.RegisterEntryPoint<InventoryService>(Lifetime.Singleton).AsSelf();

        builder.RegisterComponentInHierarchy<UserProfileView>();
        builder.RegisterComponentInHierarchy<CharacterDetailTabView>();

        builder.RegisterComponentInHierarchy<InventoryView>();
    }
}


public class MainMenuController : Service
{
    [Inject] readonly AdsManager _AdsManager;
    [Inject] readonly ProfileSerice _ProfileSerice;

    [Inject] readonly InventoryView _InventoryView;
    [Inject] readonly InventoryService _InventoryService;

    public override void PostStart()
    {
        _AdsManager.RandomInterstitalAds();

        InventoryLoad().Forget();
    }

    private async UniTask InventoryLoad()
    {
        await UniTask.WaitUntil(() => _ProfileSerice.User != null);
        await _InventoryService.Load(_ProfileSerice.User.UserId);
    }

    public override void Dispose()
    {
    }
}
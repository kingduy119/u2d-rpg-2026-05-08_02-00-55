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

    // Dictionary<string, List<RewardItem>> _IAPReward = new();

    public override void Start()
    {
    }

    public override void PostStart()
    {
        _AdsManager.RandomInterstitalAds();

        // Test Inventory
        TestInventoryLoad().Forget();
        // TestInventorySave();
    }

    private async UniTask TestInventoryLoad()
    {

        await UniTask.WaitUntil(() => _ProfileSerice.User != null);

        var playerId = _ProfileSerice.User.UserId;

        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("PlayerId is null or empty");
            return;
        }
        var data = await _InventoryService.LoadInventory(playerId);

        if (data != null)
        {
            Debug.Log($"Load Inventory Success: {data.Datas.Length} items");
            _InventoryView.FillSlots(data.Datas);
        }
        else
        {
            Debug.Log("Load Inventory Failed");
        }
    }

    // private void TestInventorySave()
    // {
    //     var playerId = _ProfileSerice.User.UserId;
    //     if (_InventoryView.Datas != null)
    //     {
    //         _InventoryService.SaveInventory(
    //             playerId,
    //             new ListInventoryData { Datas = _InventoryView.Datas },
    //             onSuccess: () =>
    //             {
    //                 Debug.Log("Save Inventory Success");
    //             })
    //             .Forget();
    //     }
    // }

    public override void Dispose()
    {
    }
}
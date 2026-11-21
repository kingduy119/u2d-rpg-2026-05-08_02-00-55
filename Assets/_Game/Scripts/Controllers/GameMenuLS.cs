using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IAPPack;
using Inventory;
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

        builder.RegisterComponentInHierarchy<InventoryView>();
    }
}


public class GameMenuController : Service
{
    [Inject] readonly AdsManager _AdsManager;
    [Inject] readonly GameManager _GameManager;
    [Inject] readonly ProfileSerice _ProfileSerice;

    [Inject] readonly InventoryView _InventoryView;
    [Inject] readonly InventoryService _InventoryService;

    Dictionary<string, List<RewardData>> _IAPReward = new();

    public override void Start()
    {
        ShopEvents.ShopItemSelect += OnShopItemSelect;
        _GameManager._IAPManager.PurchaseConfirmed += PurchaseConfirmed;
    }

    public override void PostStart()
    {
        // _AdsManager.RandomInterstitalAds();

        // Test Inventory
        TestInventoryLoad().Forget();
        // TestInventorySave();
    }

    private async UniTask TestInventoryLoad()
    {

        await UniTask.WaitUntil(
            () => _ProfileSerice.User != null
            );

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

    private void TestInventorySave()
    {
        var playerId = _ProfileSerice.User.UserId;
        if (_InventoryView.Datas != null)
        {
            _InventoryService.SaveInventory(
                playerId,
                new ListInventoryData { Datas = _InventoryView.Datas },
                onSuccess: () =>
                {
                    Debug.Log("Save Inventory Success");
                })
                .Forget();
        }
    }

    public override void Dispose()
    {
        ShopEvents.ShopItemSelect -= OnShopItemSelect;
        _GameManager._IAPManager.PurchaseConfirmed -= PurchaseConfirmed;
    }

    private void OnShopItemSelect(ShopItemDto item)
    {
        if (item == null) return;

        switch (item)
        {
            case ShopAdsItemDto adsDto:
                _AdsManager.SaveAdsRewards(adsDto.Rewards);
                AdsEvent.ShowRewardAds("RewardAds");
                break;
            case PurchaseResourceDto purchaseDto:
                ShopEvents.OnPurchase?.Invoke(purchaseDto);
                break;
            case ShopIapDto iapDto:
                var productId = iapDto.ProductId.ToString();
                var rewards = iapDto.Rewards;

                if (!_IAPReward.ContainsKey(productId))
                    _IAPReward.Add(productId, rewards);

                _GameManager._IAPManager.BuyProduct(productId);
                break;
            default: break;
        }
    }

    private void PurchaseConfirmed(string productId)
    {
        if (_IAPReward.TryGetValue(productId, out var reward))
        {
            _ProfileSerice.ClaimRewards(reward);
        }
    }


}
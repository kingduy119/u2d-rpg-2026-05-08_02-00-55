using System.Collections.Generic;
using IAPPack;
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
    // [Inject] readonly FactoryService _FactoryService;
    // [Inject] readonly GameManager _GameManager;
    [Inject] readonly AdsManager _AdsManager;
    [Inject] readonly GameManager _GameManager;
    [Inject] readonly ProfileSerice _ProfileSerice;

    Dictionary<string, List<RewardData>> _IAPReward = new();

    public override void Start()
    {
        ShopEvents.ShopItemSelect += OnShopItemSelect;
        _GameManager._IAPManager.PurchaseConfirmed += PurchaseConfirmed;
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
        Debug.Log($"_ProfileSerice.ClaimRewards: {productId}");
        if (_IAPReward.TryGetValue(productId, out var reward))
        {
            _ProfileSerice.ClaimRewards(reward);
        }
    }

    public override void PostStart()
    {
        _AdsManager.RandomInterstitalAds();
    }

}
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

    public override void Start()
    {
        ShopEvents.ShopItemSelect += OnShopItemSelect;
    }
    public override void Dispose()
    {
        ShopEvents.ShopItemSelect -= OnShopItemSelect;
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
            default: break;
        }
    }

    public override void PostStart()
    {
        _AdsManager.RandomInterstitalAds();
    }

}
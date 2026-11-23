


using System.Collections.Generic;
using AdsLib;
using Core;
using IAPLib;
using VContainer;

public class IAPController : Service
{
    [Inject] readonly IAPManager _IAPManager;
    [Inject] readonly AdsManager _AdsManager;
    [Inject] readonly ProfileSerice _ProfileSerice;
    [Inject] readonly InventoryService _InventoryService;

    Dictionary<string, List<RewardItem>> _IAPReward = new();
    List<RewardItem> AdsRewards;
    bool _sendedRewards;

    public override void Start()
    {
        ShopEvents.ShopItemSelect += OnShopItemSelect;
        _IAPManager.PurchaseConfirmed += PurchaseConfirmed;
        AdsEvent.GiveAdsReward += GiveAdsReward;
    }
    public override void Dispose()
    {
        ShopEvents.ShopItemSelect -= OnShopItemSelect;
        _IAPManager.PurchaseConfirmed -= PurchaseConfirmed;
        AdsEvent.GiveAdsReward -= GiveAdsReward;
    }

    private void OnShopItemSelect(ShopItemDto item)
    {
        if (item == null) return;

        switch (item)
        {
            case ShopAdsItemDto adsDto:
                _sendedRewards = false;
                AdsRewards = adsDto.Rewards;
                AdsEvent.ShowRewardAds("RewardAds");
                break;
            case ShopIapResourceDto resourceDto:
                _ProfileSerice.PurchaseItem(resourceDto, OnPurchaseSuccess);
                break;
            case ShopIapDto iapDto:
                var productId = iapDto.ProductId.ToString();
                var rewards = iapDto.Rewards;

                if (!_IAPReward.ContainsKey(productId))
                    _IAPReward.Add(productId, rewards);

                _IAPManager.BuyProduct(productId);
                break;
            default: break;
        }
    }

    private void OnPurchaseSuccess(List<RewardItem> rewards)
    {
        if (rewards == null || rewards.Count <= 0) return;
        foreach (var reward in rewards)
        {
            ClaimRewards(reward);
        }
    }

    private void ClaimRewards(RewardItem reward)
    {
        if (reward.Item == null || reward.Value <= 0) return;

        switch (reward.Item.Type)
        {
            case ItemType.Weapon:
                // _ProfileSerice.UserResource.Add(reward);
                _InventoryService.AddItem(reward);
                break;
            case ItemType.Gold:
                _ProfileSerice.UserResource.Add(reward);
                break;
            default: break;
        }
    }

    private void GiveAdsReward(string adsName)
    {
        if (_sendedRewards || AdsRewards == null || AdsRewards.Count <= 0) return;

        _ProfileSerice.ClaimRewards(AdsRewards);
        _sendedRewards = true;
    }

    private void PurchaseConfirmed(string productId)
    {
        if (_IAPReward.TryGetValue(productId, out var reward))
        {
            _ProfileSerice.ClaimRewards(reward);
        }
    }
}
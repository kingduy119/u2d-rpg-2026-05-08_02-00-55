

using System;
using System.Collections.Generic;
using Core;
using VContainer;
using VContainer.Unity;

public class AdsManager : AdsInitializer, IStartable, IDisposable
{
    [Inject] readonly AdsSettingSO _AdsSettings;
    [Inject] readonly ProfileSerice _ProfileSerice;

    bool _sendedRewards;
    public bool ShowAds;
    List<RewardItem> AdsRewards;

    public void Start()
    {
        AdsEvent.GiveAdsReward += GiveAdsReward;

        if (_AdsSettings != null) Init(_AdsSettings);
    }

    public void Dispose()
    {
        AdsEvent.GiveAdsReward -= GiveAdsReward;
        RemoveEvents();
    }

    public void SaveAdsRewards(List<RewardItem> rewards)
    {
        AdsRewards = rewards;
        _sendedRewards = false;
    }

    private void GiveAdsReward(string reward)
    {
        if (_sendedRewards || AdsRewards == null || AdsRewards.Count <= 0) return;

        _ProfileSerice.ClaimRewards(AdsRewards);
        _sendedRewards = true;
    }

    public void RandomInterstitalAds(int percent = 10)
    {
        if (!ShowAds) return;
        if (UnityEngine.Random.Range(0, 100) < percent)
        {
            AdsEvent.ShowInterstitial?.Invoke("");
        }
        ShowAds = false;
    }
}
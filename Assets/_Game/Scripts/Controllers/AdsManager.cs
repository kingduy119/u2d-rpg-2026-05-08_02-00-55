

using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

public class AdsManager : AdsInitializer, IStartable, IDisposable
{
    [Inject] readonly AdsSettingSO _AdsSettings;
    [Inject] readonly ProfileSerice _ProfileSerice;

    bool _sendedRewards;
    List<RewardData> AdsRewards;
    public bool ShowAds;

    public void Start()
    {
        // ButtonEvents.SaveAdsRewards += SaveAdsRewards;
        AdsEvent.GiveAdsReward += GiveAdsReward;

        if (_AdsSettings != null) Init(_AdsSettings);
    }

    public void Dispose()
    {
        // ButtonEvents.SaveAdsRewards -= SaveAdsRewards;
        AdsEvent.GiveAdsReward -= GiveAdsReward;
        Release();
    }
    public void SaveAdsRewards(List<RewardData> rewards)
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

    public void RandomInterstitalAds()
    {
        if (!ShowAds) return;
        AdsEvent.ShowInterstitial?.Invoke("");
        // if (UnityEngine.Random.Range(0, 100) < 20)
        // {
        //     AdsEvent.ShowInterstitial?.Invoke("");
        // }
        ShowAds = false;
    }
}
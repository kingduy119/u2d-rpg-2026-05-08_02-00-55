

using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class AdsManager : AdsInitializer, IStartable, IDisposable
{
    [Inject] readonly public AdsSettingSO AdsSettings;

    List<ItemSO> AdsRewards;
    bool _sendedRewards;

    public void Start()
    {
        // Debug.Log($"AdsManager.Start: {AdsSettings != null}");
        ButtonEvents.SaveAdsRewards += SaveAdsRewards;
        AdsEvent.GiveAdsReward += GiveAdsReward;

        if (AdsSettings != null) Init(AdsSettings);
    }

    public void Dispose()
    {
        AdsEvent.GiveAdsReward -= GiveAdsReward;
        Release();
    }
    private void SaveAdsRewards(List<ItemSO> rewards)
    {
        AdsRewards = rewards;
        _sendedRewards = false;
    }

    private void GiveAdsReward(string reward)
    {
        if (!_sendedRewards)
        {
            ButtonEvents.ClaimRewards?.Invoke(AdsRewards);
            _sendedRewards = true;
        }

    }
}
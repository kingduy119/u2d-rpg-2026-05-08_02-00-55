

using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class AdsManager : AdsInitializer, IStartable, IDisposable
{
    [Inject] readonly public AdsSettingSO AdsSettings;


    public void Start()
    {
        // Debug.Log($"AdsManager.Start: {AdsSettings != null}");
        AdsEvent.GiveAdsReward += GiveAdsReward;

        if (AdsSettings != null) Init(AdsSettings);
    }

    public void Dispose()
    {
        AdsEvent.GiveAdsReward -= GiveAdsReward;
        Release();
    }

    private void GiveAdsReward(string reward)
    {
        Debug.Log($"GiveAdsReward: {reward}");
    }
}
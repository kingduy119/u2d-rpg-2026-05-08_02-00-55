

using System;
using UnityEngine.UI;

public static class AdsEvent
{
    public static Action ShowBanner;
    public static Action HideBanner;
    public static Action ShowInterstitial;
    public static Action ShowRewardAds;
    public static Action<string> GiveAdsReward;
    public static Action<string> ShowAdsFailure;
    public static Action<string> ShowAdsStart;
    public static Action<string> ShowAdsClick;
}
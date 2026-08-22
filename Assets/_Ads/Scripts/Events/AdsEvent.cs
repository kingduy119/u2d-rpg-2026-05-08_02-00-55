

using System;
using UnityEngine.UI;

public static class AdsEvent
{
    public static Action<Button> ShowInterstitial;
    public static Action<Button> ShowRewardAds;
    public static Action ShowBanner;
    public static Action HideBanner;

}
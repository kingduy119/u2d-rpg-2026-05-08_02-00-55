using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : IUnityAdsInitializationListener
{
    private AdsSettingSO _settings;
    public bool InitCompleted;
    private IBanner _banner;
    private IAdvertise _rewardedAds;
    private IAdvertise _interstitialAds;

    public void Init(AdsSettingSO settings)
    {
        _settings = settings;
        if (_settings != null && !Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_settings.GameID, _settings.TestMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        _interstitialAds = new Advertise(_settings.InterstitialAdUnitId, true);
        _rewardedAds = new Advertise(_settings.RewardedAdUnitId, true);
        _banner = new Banner(_settings.BannerPosition, _settings.BannerAdUnitId);

        InitCompleted = true;
        RegisteEvents();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        InitCompleted = false;
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void RegisteEvents()
    {
        if (!InitCompleted) return;
        AdsEvent.ShowInterstitial += _interstitialAds.Show;
        AdsEvent.ShowRewardAds += _rewardedAds.Show;
        AdsEvent.ShowBanner += _banner.Show;
        AdsEvent.HideBanner += _banner.Hide;
    }

    public void Release()
    {
        if (!InitCompleted) return;
        AdsEvent.ShowInterstitial -= _interstitialAds.Show;
        AdsEvent.ShowRewardAds -= _rewardedAds.Show;
        AdsEvent.ShowBanner -= _banner.Show;
        AdsEvent.HideBanner -= _banner.Hide;
    }
}

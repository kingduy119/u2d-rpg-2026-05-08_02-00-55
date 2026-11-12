using UnityEngine;



public class AdsManagerView : MonoBehaviour
{

    // [SerializeField] AdsConfigView Configs;
    // private IBanner _banner;
    // private IAdvertise _rewardedAds;
    // private IAdvertise _interstitialAds;
    [SerializeField] AdsSettingSO _adsSettings;
    private readonly AdsInitializer _AdsInitializer = new();

    private void Awake()
    {
        if (_adsSettings != null)
        {
            _AdsInitializer.Init(_adsSettings);
        }
        // if (TryGetComponent(out AdsConfigView configs))
        // {
        //     // Configs = configs;
        //     // _AdsInitializer = new AdsInitializer(Configs.GameID, Configs.TestMode);

        //     // _interstitialAds = new Advertise(Configs.InterstitialAdUnitId, true);
        //     // _rewardedAds = new Advertise(Configs.RewardedAdUnitId, true);
        //     // _banner = new Banner(Configs.BannerPosition, Configs.BannerAdUnitId);
        // }
        // else
        // {
        //     Debug.LogError("[AdsManager] AdsConfigView component is missing.");
        // }
    }

    void OnEnable()
    {
        // Debug.Log("AdsManager.OnEnable");
        // AdsEvent.ShowInterstitial += _interstitialAds.Show;
        // AdsEvent.ShowRewardAds += _rewardedAds.Show;
        // AdsEvent.ShowBanner += _banner.Show;
        // AdsEvent.HideBanner += _banner.Hide;
    }

    void OnDisable()
    {
        _AdsInitializer.Release();
        // AdsEvent.ShowInterstitial -= _interstitialAds.Show;
        // AdsEvent.ShowRewardAds -= _rewardedAds.Show;
        // AdsEvent.ShowBanner -= _banner.Show;
        // AdsEvent.HideBanner -= _banner.Hide;
    }
}

using UnityEngine;



[RequireComponent(typeof(AdsConfigView))]
public class AdsManager : MonoBehaviour
{

    public AdsConfigView Configs { get; private set; }
    private AdsInitializer _AdsInitializer;
    private IBanner _banner;
    private IAdvertise _rewardedAds;
    private IAdvertise _interstitialAds;

    private void Awake()
    {
        if (TryGetComponent(out AdsConfigView configs))
        {
            Configs = configs;
            _AdsInitializer = new AdsInitializer(Configs.GameID, Configs.TestMode);

            _interstitialAds = new Advertise(Configs.InterstitialAdUnitId, true);
            _rewardedAds = new Advertise(Configs.RewardedAdUnitId, true);
            _banner = new Banner(Configs.BannerPosition, Configs.BannerAdUnitId);
        }
        else
        {
            Debug.LogError("[AdsManager] AdsConfigView component is missing.");
        }
    }

    void OnEnable()
    {
        Debug.Log("AdsManager.OnEnable");
        AdsEvent.ShowInterstitial += _interstitialAds.Show;
        AdsEvent.ShowRewardAds += _rewardedAds.Show;
        AdsEvent.ShowBanner += _banner.Show;
        AdsEvent.HideBanner += _banner.Hide;
    }

    void OnDisable()
    {
        AdsEvent.ShowInterstitial -= _interstitialAds.Show;
        AdsEvent.ShowRewardAds -= _rewardedAds.Show;
        AdsEvent.ShowBanner -= _banner.Show;
        AdsEvent.HideBanner -= _banner.Hide;
    }
}
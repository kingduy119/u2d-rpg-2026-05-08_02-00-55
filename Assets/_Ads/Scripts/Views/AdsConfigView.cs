


using UnityEngine;
using UnityEngine.Advertisements;

public class AdsConfigView : MonoBehaviour
{
    public bool TestMode = true;

    [Header("GameID")]
    public string _androidGameId;
    public string _iOSGameId;

    [Header("Interstitial Ads")]
    public string Interstitial_Android = "Interstitial_Android";
    public string Interstitial_iOS = "Interstitial_iOS";

    [Header("Rewarded Ads")]
    public string Rewarded_Android = "Rewarded_Android";
    public string Rewarded_iOS = "Rewarded_iOS";

    [Header("Banner Ads")]
    public string Banner_Android = "Banner_Android";
    public string Banner_iOS = "Banner_iOS";
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;


    private bool IOS => Application.platform == RuntimePlatform.IPhonePlayer;

    public string GameID => IOS ? _iOSGameId : _androidGameId;

    public string InterstitialAdUnitId => IOS ? Interstitial_iOS : Interstitial_Android;

    public string RewardedAdUnitId => IOS ? Rewarded_iOS : Rewarded_Android;

    public string BannerAdUnitId => IOS ? Banner_iOS : Banner_Android;
    public BannerPosition BannerPosition => _bannerPosition;
}
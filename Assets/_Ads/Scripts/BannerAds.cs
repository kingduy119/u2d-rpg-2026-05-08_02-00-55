using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms.
    public bool IsLoaded { get; private set; }

    void Start()
    {
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSAdUnitId : _androidAdUnitId;
        Advertisement.Banner.SetPosition(_bannerPosition);
        LoadAd();
    }

    void OnEnable()
    {
        // AdsEvent.ShowInterstitial += AdsEvent_ShowInterstitial;
        AdsEvent.ShowBanner += AdsEvent_ShowBanner;
        AdsEvent.HideBanner += AdsEvent_HideBanner;
    }
    void OnDisable()
    {
        // AdsEvent.ShowInterstitial -= AdsEvent_ShowInterstitial;
        AdsEvent.ShowBanner -= AdsEvent_ShowBanner;
        AdsEvent.HideBanner -= AdsEvent_HideBanner;
    }

    // Implement a method to call when the Load Banner button is clicked:
    private void LoadAd()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new()
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);
    }
    void OnBannerLoaded() => IsLoaded = true;
    void OnBannerError(string message) => IsLoaded = false;

    private void AdsEvent_ShowBanner()
    {
        if (!IsLoaded) return;

        ShowBannerAd();
    }

    // Implement a method to call when the Show Banner button is clicked:
    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new()
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId, options);
    }

    // Implement a method to call when the Hide Banner button is clicked:
    // HideBannerAd
    void AdsEvent_HideBanner() => Advertisement.Banner.Hide();

    void OnBannerClicked() { Debug.Log("OnBannerClicked"); }
    void OnBannerShown() { Debug.Log("OnBannerShown"); }
    void OnBannerHidden() { Debug.Log("OnBannerHidden"); }

    // void OnDestroy()
    // {
    //     // Clean up the listeners:
    //     // _loadBannerButton.onClick.RemoveAllListeners();
    //     // _showBannerButton.onClick.RemoveAllListeners();
    //     // _hideBannerButton.onClick.RemoveAllListeners();
    // }
}

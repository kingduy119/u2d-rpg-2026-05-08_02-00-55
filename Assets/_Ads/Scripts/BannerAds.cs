using UnityEngine;
using UnityEngine.Advertisements;

public interface IBanner
{
    void Hide();
    void Show();
}
public class Banner : IBanner
{
    bool IsLoaded;
    private string _BannerAdUnitId;
    private BannerPosition _bannerPosition;
    public Banner(BannerPosition position, string bannerAdUnitId)
    {
        _bannerPosition = position;
        _BannerAdUnitId = bannerAdUnitId;
        Advertisement.Banner.SetPosition(_bannerPosition);

        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new()
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.Load(_BannerAdUnitId, options);
    }

    public void Hide() => Advertisement.Banner.Hide();
    public void Show() { if (IsLoaded) ShowBannerAd(); }

    void OnBannerLoaded()
    {
        IsLoaded = true;
        Debug.Log($"Banner Ad Loaded: {_BannerAdUnitId}");
    }
    void OnBannerError(string message)
    {
        IsLoaded = false;
        Debug.LogError($"Banner Ad Failed to Load: {_BannerAdUnitId} - {message}");
    }

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
        Advertisement.Banner.Show(_BannerAdUnitId, options);
    }

    // Implement a method to call when the Hide Banner button is clicked:
    void OnBannerClicked() { Debug.Log("OnBannerClicked"); }
    void OnBannerShown() { Debug.Log("OnBannerShown"); }
    void OnBannerHidden() { Debug.Log("OnBannerHidden"); }
}


using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
, IUnityAdsInitializationListener
// , IUnityAdsLoadListener
// , IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    [SerializeField] string _adUnitId = "Rewarded_Android";

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("[AdsManager] Unity Ads initialization complete.");
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"[AdsManager] OnInitializationFailed: {message}");
    }
}
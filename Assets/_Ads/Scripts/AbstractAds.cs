using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AbstractAds : MonoBehaviour
, IUnityAdsLoadListener
, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOSAdUnitId = "Interstitial_iOS";
    protected string _adUnitId;
    public bool IsLoaded { get; private set; }

    protected void Start()
    {
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
        ? _iOSAdUnitId
        : _androidAdUnitId;

        StartCoroutine(LoadAd());
    }

    protected IEnumerator LoadAd()
    {
        Debug.Log("LoadAd: " + _adUnitId);
        yield return new WaitForSeconds(1f);
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        IsLoaded = true;
        Debug.Log($"OnUnityAdsAdLoaded: {placementId}");
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        IsLoaded = false;
        Debug.Log($"OnUnityAdsFailedToLoad: {placementId} - {message}");
    }


    // InterstitialAds
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        IsLoaded = false;
        Debug.Log($"OnUnityAdsShowFailure: {placementId}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"OnUnityAdsShowClick: {placementId}");
    }

    public virtual void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        // throw new System.NotImplementedException();
        if (placementId.Equals(_adUnitId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Payouts to players here");
        }
    }
}
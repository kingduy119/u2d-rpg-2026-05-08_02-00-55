using UnityEngine;
using UnityEngine.Advertisements;

public interface IAdvertise
{
    // void LoadAd(string adUnitId);
    void Show(string reward = "empty");
}

// Interstitial & Rewarded
public class Advertise : IAdvertise
, IUnityAdsLoadListener
, IUnityAdsShowListener
{
    private readonly string AdUnitId;
    bool IsDebug = false;
    string _reward;

    public Advertise(string adUnitId, bool isDebug = false)
    {
        AdUnitId = adUnitId;
        IsDebug = isDebug;
        Advertisement.Load(AdUnitId, this);
    }

    public void Show(string reward = "empty")
    {
        _reward = reward;
        Advertisement.Show(AdUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (IsDebug) Debug.Log($"OnUnityAdsAdLoaded: {placementId}");
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (IsDebug) Debug.Log($"OnUnityAdsFailedToLoad: {placementId} - {message}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        if (IsDebug) Debug.Log($"OnUnityAdsShowClick: {placementId}");
        AdsEvent.ShowAdsClick?.Invoke(placementId);
    }

    public virtual void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(AdUnitId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            if (IsDebug) Debug.Log($" {placementId} Payouts to players here");

            AdsEvent.GiveAdsReward?.Invoke(_reward);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (IsDebug) Debug.Log($"OnUnityAdsShowFailure: {placementId}");
        AdsEvent.ShowAdsFailure?.Invoke(placementId);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        if (IsDebug) Debug.Log($"OnUnityAdsShowStart: {placementId}");
        AdsEvent.ShowAdsStart?.Invoke(placementId);
    }
}


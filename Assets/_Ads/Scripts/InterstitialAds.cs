using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;



public class InterstitialAds : AbstractAds
{
    Button _adButton;

    void OnEnable()
    {
        AdsEvent.ShowInterstitial += AdsEvent_ShowInterstitial;
    }
    void OnDisable()
    {
        AdsEvent.ShowInterstitial -= AdsEvent_ShowInterstitial;
    }

    // Show the loaded content in the Ad Unit:
    private void AdsEvent_ShowInterstitial(Button button)
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log($"AdsEvent_ShowInterstitial: {_adUnitId} - {IsLoaded}");
        if (!IsLoaded) return;

        _adButton = button;
        _adButton.interactable = false;
        Advertisement.Show(_adUnitId, this);
    }

    public override void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        base.OnUnityAdsShowComplete(placementId, showCompletionState);
        _adButton.interactable = true;
    }

}

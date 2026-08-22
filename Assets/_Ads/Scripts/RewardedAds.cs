using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAds : AbstractAds
{
    Button _adButton;

    void OnEnable()
    {
        AdsEvent.ShowRewardAds += AdsEvent_ShowRewardAds;
    }

    void OnDisable()
    {
        AdsEvent.ShowRewardAds -= AdsEvent_ShowRewardAds;
    }

    // Implement a method to execute when the user clicks the button:
    private void AdsEvent_ShowRewardAds(Button button)
    {
        Advertisement.Show(_adUnitId, this);
        _adButton = button;
        _adButton.interactable = false;
    }

    public override void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        base.OnUnityAdsShowComplete(placementId, showCompletionState);
        _adButton.interactable = true;
    }

}

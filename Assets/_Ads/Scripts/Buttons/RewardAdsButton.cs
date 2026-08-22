

namespace Ads
{
    public class RewardAdsButton : AbstractButton
    {
        protected override void HandleClick()
        {
            AdsEvent.ShowRewardAds?.Invoke(_Button);
            // _Button.interactable = false;
        }
    }
}

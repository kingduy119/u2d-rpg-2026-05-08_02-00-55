

namespace Ads
{
    public class InterstitialAdsButton : AbstractButton
    {
        protected override void HandleClick()
        {
            AdsEvent.ShowInterstitial?.Invoke(_Button);
        }
    }
}

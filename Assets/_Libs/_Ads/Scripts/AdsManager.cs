

namespace AdsLib
{

    public class AdsManager : AdsInitializer
    {
        public bool ShowAds;

        public void RandomInterstitalAds(int percent = 10)
        {
            if (!ShowAds) return;
            if (UnityEngine.Random.Range(0, 100) < percent)
            {
                AdsEvent.ShowInterstitial?.Invoke("");
            }
            ShowAds = false;
        }
    }
}

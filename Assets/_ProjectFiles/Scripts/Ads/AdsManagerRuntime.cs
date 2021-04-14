using GoogleMobileAds.Api;
using UnityEngine;

namespace Game.Ads
{
    public static partial class AdsManager
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AdsManagerConstructor()
        {
            MobileAds.Initialize(x => { });
        }

        public static BannerView RequestBanner()
        {
            var unitId = GetUnitId(AdType.Banner);
            var bannerView = new BannerView(unitId, AdSize.Banner, AdPosition.Top);
            bannerView.LoadAd(new AdRequest.Builder().Build());

            return bannerView;
        }

        public static InterstitialAd RequestInterstitial()
        {
            var unitId = GetUnitId(AdType.Interstitial);
            var interstitial = new InterstitialAd(unitId);
            interstitial.LoadAd(new AdRequest.Builder().Build());
            
            return interstitial;
        }

        public static RewardedAd RequestRewarded()
        {
            var unitId = GetUnitId(AdType.Rewarded);
            var rewardedAd = new RewardedAd(unitId);
            rewardedAd.LoadAd(new AdRequest.Builder().Build());
            
            return rewardedAd;
        }

        private static void InterOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            MonoBehaviour.print(e.Message);
        }
    }
}
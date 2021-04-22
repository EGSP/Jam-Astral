using GoogleMobileAds.Api;

namespace Game.Ads
{
    public enum BuildType
    {
        Test,
        Publish
    }
    
    /// <summary>
    /// Содержит функционал запроса рекламы и предоставления данных для ее инициализации.
    /// </summary>
    public static partial class AdsManager
    {
        public const string UnexpectedPlatformUnitId = "unexpected_platform";

        public static BuildType BuildType { get; set; } = BuildType.Test;

        public enum AdType
        {
            Banner,
            Interstitial,
            Native,
            Rewarded
        }

        public enum MobileOs
        {
            Android,
            IOS,
            Unexpected
        }
        
        public static string GetUnitId(AdType ad)
        {
            switch (GetOs())
            {
                case MobileOs.Android:
                    return GetAndroidUnitId(ad);
            }
            
            return UnexpectedPlatformUnitId;
        }

        private static string GetAndroidUnitId(AdType ad)
        {
            if (BuildType == BuildType.Publish)
                return UnexpectedPlatformUnitId;
            
            switch (ad)
            {
                case AdType.Banner:
                    return "ca-app-pub-3940256099942544/6300978111";
                case AdType.Interstitial:
                    return "ca-app-pub-3940256099942544/1033173712";
                case AdType.Native:
                    return "ca-app-pub-3940256099942544/2247696110";
                case AdType.Rewarded:
                    return "ca-app-pub-3940256099942544/5224354917";
            }

            return UnexpectedPlatformUnitId;
        }

        public static MobileOs GetOs()
        {
#if UNITY_ANDROID
            return MobileOs.Android;
#elif UNITY_IOS
            return MobileOs.IOS;
#else
            return MobileOs.Unexpected;
#endif
        } 

    }
}
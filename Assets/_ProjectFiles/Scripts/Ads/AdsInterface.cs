using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Ads
{
    public class AdsInterface : MonoBehaviour
    {
        [SerializeField] private Button bannerButton;
        [SerializeField] private Button closeBunnerButton;
        
        [SerializeField] private Button interstitialButton;
        
        [SerializeField] private Button rewardedButton;
        
        private BannerView _bannerView;
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;

        private Queue<Action> _actions = new Queue<Action>();
        
        private void Update()
        {
            EventQueue();
        }

        private void EventQueue()
        {
            if (_actions.Count <= 0)
                return;

            // Сделано через цикл, т.к. происходит запись из другого потока (потока рекламы).
            while (_actions.Count > 0)
            {
                var action = _actions.Dequeue();
                action?.Invoke();   
            }
        }

        public void ShowBanner()
        {
            _bannerView = AdsManager.RequestBanner();
            _bannerView.OnAdLoaded += BannerViewOnAdLoaded;
            LockButton(bannerButton);
            
            void BannerViewOnAdLoaded(object sender, EventArgs e)
            {
                var banner = sender as BannerView;
                if (banner != null)
                {
                    UnlockButtonThreadSafe(closeBunnerButton);
                    banner.Show();
                    banner.OnAdLoaded -= BannerViewOnAdLoaded;
                    banner.OnAdClosed += BannerOnAdClosed;
                }
            }
        }
        
        // Событие баннера не вызывается при тестовом режиме.
        void BannerOnAdClosed(object sender, EventArgs e)
        {
            var banner = sender as BannerView;
            if (banner != null)
            {
                LockButtonThreadSafe(closeBunnerButton);
                UnlockButtonThreadSafe(bannerButton);
                banner.OnAdClosed -= BannerOnAdClosed;
            }
        }

        public void CloseBanner()
        {
            if (_bannerView != null)
            {
                LockButton(closeBunnerButton);
                UnlockButton(bannerButton);
                _bannerView.OnAdClosed -= BannerOnAdClosed;
            }
            
            _bannerView?.Hide();
            _bannerView?.Destroy();
        }

        private void LockButton(Button button)
        {
            if (button != null)
                button.interactable = false;
        }

        private void LockButtonThreadSafe(Button button)
        {
            _actions.Enqueue(() => LockButton(button));
        }

        private void UnlockButton(Button button)
        {
            if (button != null)
                button.interactable = true;
        }

        private void UnlockButtonThreadSafe(Button button)
        {
            _actions.Enqueue(() => UnlockButton(button));
        }

        private void UnlockAllButtons()
        {
            UnlockButton(bannerButton);
            UnlockButton(interstitialButton);
            UnlockButton(rewardedButton);
        }
        
        
        public void CloseAll()
        {
            if (_bannerView != null)
            {
                _bannerView.Destroy();
                _bannerView = null;
            }

            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            if (_rewardedAd != null)
            {
                _rewardedAd = null;
            }
            
            UnlockAllButtons();
        }
    }
}
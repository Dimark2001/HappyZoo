using UnityEngine;
using CAS;
using CAS.UserConsent;

public class CASManager : MonoBehaviour
{
	private IMediationManager _manager;

	public int IterstitialTimer = 45;

	private float _timer;
	private bool _canShowInter;

	private void Start()
	{
		_timer = IterstitialTimer;
		CAS.MobileAds.settings.isExecuteEventsOnUnityThread = true;

		ConsentRequestParameters request = CAS.UserConsent.UserConsent.BuildRequest();
		request.WithCallback(Init);
		request.Present();

		DontDestroyOnLoad(this);
	}

	private void Init()
	{
		if (CAS.UserConsent.UserConsent.GetStatus() == ConsentStatus.Accepted)
			CAS.MobileAds.settings.userCCPAStatus = CCPAStatus.OptInSale;

		_manager = MobileAds.BuildManager()
			// Select first Manager Id from list in settings asset
			.WithManagerIdAtIndex(0)
			// Call Initialize method in any case to get IMediationManager instance
			.Initialize();

		//CAS.MobileAds.ValidateIntegration();

		IAdView adView = _manager.GetAdView(AdSize.Banner);
		adView.Load();
		adView.SetActive(true);
		//CAS.MobileAds.settings.interstitialInterval = IterstitialTimer;
		//CAS.MobileAds.settings.RestartInterstitialInterval();
		_manager.OnInterstitialAdLoaded += CanShowInter;
		_manager.OnInterstitialAdClosed += InterShown;
	}

	private void CanShowInter()
	{
		_canShowInter = true;
	}
	private void InterShown()
	{
		_canShowInter = false;
		_timer = IterstitialTimer;
	}

	private void Update()
	{
		_timer -= Time.deltaTime;
		if (_timer < 0)
		{
			_canShowInter = false;
			ShowInter();
		}
	}
	private void ShowInter()
	{
		_manager.ShowAd(AdType.Interstitial);
	}
}

using UnityEngine;
using System;
using System.Threading;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
	[SerializeField] bool isRaunch;
	[SerializeField] bool isDetailLog;
	[SerializeField] string[] testDeviceIds;
	[SerializeField] string bannerIdAndroid;
	[SerializeField] string bannerIdIos;
	[SerializeField] string frontIdAndroid;
	[SerializeField] string frontIdIos;
	[SerializeField] string rewardIdAndroid;
	[SerializeField] string rewardIdIos;

	SynchronizationContext context;



	void Start()
	{
		context = SynchronizationContext.Current;
		if (IsAndroidAPK() && isRaunch)
		{
			isRaunch = false;
			if (isDetailLog)
			{
				Debug.Log("Android APK detected, disabling raunch mode.");
			}
		}

		RequestConfiguration requestConfiguration = new RequestConfiguration();
		requestConfiguration.TestDeviceIds.AddRange(testDeviceIds);
		MobileAds.SetRequestConfiguration(requestConfiguration);
		MobileAds.Initialize(initStatus =>
		{
			if (!string.IsNullOrEmpty(bannerIdAndroid) || !string.IsNullOrEmpty(bannerIdIos))
			{
				LoadBannerAd();
			}
			if (!string.IsNullOrEmpty(frontIdAndroid) || !string.IsNullOrEmpty(frontIdIos))
			{
				LoadFrontAd();
			}
			if (!string.IsNullOrEmpty(rewardIdAndroid) || !string.IsNullOrEmpty(rewardIdIos))
			{
				LoadRewardAd();
			}
		});
	}

	bool IsAndroidAPK()
	{
		string bundleId = Application.identifier;
		return bundleId.EndsWith(".apk");
	}





	#region Banner Ad

	BannerView bannerAd;

	public void LoadBannerAd()
	{
#if UNITY_ANDROID
		string bannerId = bannerIdAndroid;
		string bannerIdTest = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE || UNITY_IOS
		string bannerId = bannerIdIos;
		string bannerIdTest = "ca-app-pub-3940256099942544/2934735716";
#endif
		string finalBannerId = isRaunch ? bannerId : bannerIdTest;

		if (bannerAd != null)
		{
			bannerAd.Destroy();
		}

		bannerAd = new BannerView(finalBannerId, AdSize.Banner, AdPosition.Bottom);
		bannerAd.LoadAd(new AdRequest());

		if (isDetailLog)
		{
			Debug.Log("Banner ad loading...");
		}
	}

	public void ShowBannerAd(bool show)
	{
		if (bannerAd != null)
		{
			if (show)
			{
				bannerAd.Show();
			}
			else
			{
				bannerAd.Hide();
			}
		}
		else
		{
			if (isDetailLog)
			{
				Debug.Log("Banner ad not loaded.");
			}
			if (show)
			{
				LoadBannerAd();
			}
		}
	}

	#endregion





	#region Front Ad

	InterstitialAd frontAd;

	public void LoadFrontAd()
	{
#if UNITY_ANDROID
		string frontId = frontIdAndroid;
		string frontIdTest = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE || UNITY_IOS
		string frontId = frontIdIos;
		string frontIdTest = "ca-app-pub-3940256099942544/4411468910";
#endif
		string finalFrontId = isRaunch ? frontId : frontIdTest;
		InterstitialAd.Load(finalFrontId, new AdRequest(), (InterstitialAd ad, LoadAdError loadError) =>
		{
			if (ad == null || loadError != null)
			{
				if (isDetailLog)
				{
					Debug.Log($"Interstitial ad failed to load: {loadError?.GetMessage()}");
				}
				return;
			}
			if (isDetailLog)
			{
				Debug.Log("Interstitial ad loaded.");
			}
			frontAd = ad;
		});
	}

	public void ShowFrontAd()
	{
		if (frontAd != null && frontAd.CanShowAd())
		{
			frontAd.Show();
		}
		else
		{
			if (isDetailLog)
			{
				Debug.Log("Interstitial ad cannot be shown.");
			}
		}

		LoadFrontAd();
	}
	#endregion





	#region Reward Ad

	RewardedAd rewardAd;

	public void LoadRewardAd()
	{
#if UNITY_ANDROID
		string rewardId = rewardIdAndroid;
		string rewardIdTest = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE || UNITY_IOS
		string rewardId = rewardIdIos;
		string rewardIdTest = "ca-app-pub-3940256099942544/1712485313";
#endif
		string finalRewardId = isRaunch ? rewardId : rewardIdTest;
		RewardedAd.Load(finalRewardId, new AdRequest(),
			(RewardedAd ad, LoadAdError loadError) =>
			{
				if (ad == null || loadError != null)
				{
					if (isDetailLog)
					{
						Debug.Log($"Rewarded ad failed to load: {loadError?.GetMessage()}");
					}
					return;
				}
				if (isDetailLog)
				{
					Debug.Log("Rewarded ad loaded.");
				}
				rewardAd = ad;
			});
	}

	public void ShowRewardAd(Action<bool> completed)
	{
		if (rewardAd != null && rewardAd.CanShowAd())
		{
			rewardAd.Show((Reward reward) =>
			{
				context.Post(o =>
				{
					if (isDetailLog)
					{
						Debug.Log($"Rewarded ad granted a reward: {reward.Amount}");
					}
					completed?.Invoke(true);
				}, null);
			});
		}
		else
		{
			if (isDetailLog)
			{
				Debug.Log("Rewarded ad cannot be shown.");
			}
			completed?.Invoke(false);
		}

		LoadRewardAd();
	}

	#endregion
}
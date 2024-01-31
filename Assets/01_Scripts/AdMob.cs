using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//애드몹 관련
using GoogleMobileAds;
using GoogleMobileAds.Api; 

public class AdMob : MonoBehaviour
{
    

    public int gold; // 재화 

    // These ad units are configured to always serve test ads.
    #if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    #elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
    #else
    private string _adUnitId = "unused";
    #endif

    private RewardedAd rewardedAd; // 보상 

    // Start is called before the first frame update
    void Start()
    {
        // Google Mobile Ads SDK init 
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
        });
    }

}


 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // text ≈◊Ω∫∆Æ ¿ß«‘ 
//æ÷µÂ∏˜ ∞¸∑√
using GoogleMobileAds;
using GoogleMobileAds.Api; 

public class AdManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static AdManager instance;  // Singleton instance
    void Awake() // SingleTon
    {
        // ¿ÃπÃ ¿ŒΩ∫≈œΩ∫∞° ¡∏¿Á«œ∏Èº≠ ¿Ã∞‘ æ∆¥œ∏È ∆ƒ±´ π›»Ø
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    RewardedAd rewardedAd;
    const string videoAd = "ca-app-pub-9333309559865878/9920625966"; // ¡¯¬• ±§∞Ì ID
    const string videoAdTest = "ca-app-pub-3940256099942544/5224354917";

    const string goldAdId = "ca-app-pub-9333309559865878/2838210859"; // ≈◊Ω∫∆Æ ±§∞Ì ID
    //const string jellyAdId = "ca-app-pub-9333309559865878/3299614974"; // ≈◊Ω∫∆Æ ±§∞Ì ID


    void Start()
    {
        // Google Mobile Ads SDK init 
        MobileAds.Initialize((InitializationStatus initStatus) => {}); // √ ±‚»≠ 

        var requestConfiguration = new RequestConfiguration.Builder()
           .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        Debug.Log("±§∞Ì ¡ÿ∫Ò ");
        LoadAds();

    }


    //±§∞Ì √ ±‚»≠ «‘ºˆ
    public void LoadAds()
    {
        RewardedAd.Load(videoAdTest, new AdRequest.Builder().Build(), LoadCallback);
        Debug.Log("±§∞Ì init µ  "); 
    }


    //∑ŒµÂ ƒ›πÈ «‘ºˆ
    public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    {
        if (rewardedAd != null)
        {
            this.rewardedAd = rewardedAd;
            Debug.Log("∑ŒµÂº∫∞¯");
        }
        else
        {
            Debug.Log(loadAdError.GetMessage());
        }

    }

    //±§∞Ì ∫∏ø©¡÷¥¬ «‘ºˆ
    public void ShowAds(int rewardType)
    {
        if (rewardedAd.CanShowAd() && rewardType == 0 && DayManager.Instance.goldAd > 0)
        {
            rewardedAd.Show(GetGoldReward);
            Debug.Log("±§∞Ì ∫∏ø©¡‹ ");
        }
        else if (rewardedAd.CanShowAd() && rewardType == 1 && DayManager.Instance.jellyStoneAd > 0)
        {
            rewardedAd.Show(GetJellyReward);
            Debug.Log("±§∞Ì ∫∏ø©¡‹ ");
        }
        else if (rewardedAd.CanShowAd() && rewardType == 2 && DayManager.Instance.actionPointAd > 0)
        {
            rewardedAd.Show(GetActionPointReward);
            Debug.Log("±§∞Ì ∫∏ø©¡‹ ");
        }
        else
        {
            Debug.Log("±§∞Ì ¿Áª˝ Ω«∆–");
        }
    }

    public void GetGoldReward(Reward reward)
    {
        Debug.Log("∞ÒµÂ »πµÊ«‘");

        CurrenyManager.Instance.gold += 100; // ¿œ¥‹ 100∞ÒµÂ ¡‡ ∫Ω 
        DayManager.Instance.goldAd--; // «—∞≥ ±Ô¿Ω

        DataManager.Instance.JsonSave(); // πŸæ∆∑Œ ¿˙¿Â 
        UIManager.instance.AsycCurrenyUI();

        LoadAds();
    }

    public void GetJellyReward(Reward reward)
    {
        Debug.Log("¡©∏Æ »πµÊ«‘");

        CurrenyManager.Instance.jellyStone += 100; // ¿œ¥‹ 100∞ÒµÂ ¡‡ ∫Ω 
        DayManager.Instance.jellyStoneAd--; // «—∞≥ ±Ô¿Ω

        DataManager.Instance.JsonSave(); // ¿˙¿Â 
        UIManager.instance.AsycCurrenyUI();
        LoadAds();
    }

    public void GetActionPointReward(Reward reward)
    {
        Debug.Log("«‡µø∑¬ »πµÊ«‘");

        CurrenyManager.Instance.actionPoint += 20; //  
        if (CurrenyManager.Instance.actionPoint > 180) CurrenyManager.Instance.actionPoint = 180;
        DayManager.Instance.actionPointAd--; // «—∞≥ ±Ô¿Ω

        DataManager.Instance.JsonSave(); // πŸæ∆∑Œ ¿˙¿Â 
        UIManager.instance.AsycCurrenyUI();

        LoadAds();
    }
}

 


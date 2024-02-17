
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // text 테스트 위함 
//애드몹 관련
using GoogleMobileAds;
using GoogleMobileAds.Api; 

public class AdManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static AdManager instance;  // Singleton instance
    void Awake() // SingleTon
    {
        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
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
    const string videoAd = "ca-app-pub-9333309559865878/9920625966"; // 진짜 광고 ID
    const string videoAdTest = "ca-app-pub-3940256099942544/5224354917";

    const string goldAdId = "ca-app-pub-9333309559865878/2838210859"; // 테스트 광고 ID
    //const string jellyAdId = "ca-app-pub-9333309559865878/3299614974"; // 테스트 광고 ID


    void Start()
    {
        // Google Mobile Ads SDK init 
        MobileAds.Initialize((InitializationStatus initStatus) => {}); // 초기화 

        var requestConfiguration = new RequestConfiguration.Builder()
           .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        Debug.Log("광고 준비 ");
        LoadAds();

    }


    //광고 초기화 함수
    public void LoadAds()
    {
        RewardedAd.Load(videoAdTest, new AdRequest.Builder().Build(), LoadCallback);
        Debug.Log("광고 init 됨 "); 
    }


    //로드 콜백 함수
    public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    {
        if (rewardedAd != null)
        {
            this.rewardedAd = rewardedAd;
            Debug.Log("로드성공");
        }
        else
        {
            Debug.Log(loadAdError.GetMessage());
        }

    }

    //광고 보여주는 함수
    public void ShowAds(int rewardType)
    {
        if (rewardedAd.CanShowAd() && rewardType == 0 && DayManager.Instance.goldAd > 0)
        {
            rewardedAd.Show(GetGoldReward);
            Debug.Log("광고 보여줌 ");
        }
        else if (rewardedAd.CanShowAd() && rewardType == 1 && DayManager.Instance.jellyStoneAd > 0)
        {
            rewardedAd.Show(GetJellyReward);
            Debug.Log("광고 보여줌 ");
        }
        else if (rewardedAd.CanShowAd() && rewardType == 2 && DayManager.Instance.actionPointAd > 0)
        {
            rewardedAd.Show(GetActionPointReward);
            Debug.Log("광고 보여줌 ");
        }
        else
        {
            Debug.Log("광고 재생 실패");
        }
    }

    public void GetGoldReward(Reward reward)
    {
        Debug.Log("골드 획득함");

        CurrenyManager.Instance.gold += 100; // 일단 100골드 줘 봄 
        DayManager.Instance.goldAd--; // 한개 깍음

        DataManager.Instance.JsonSave(); // 바아로 저장 
        UIManager.instance.AsycCurrenyUI();

        LoadAds();
    }

    public void GetJellyReward(Reward reward)
    {
        Debug.Log("젤리 획득함");

        CurrenyManager.Instance.jellyStone += 100; // 일단 100골드 줘 봄 
        DayManager.Instance.jellyStoneAd--; // 한개 깍음

        DataManager.Instance.JsonSave(); // 바아로 저장 
        UIManager.instance.AsycCurrenyUI();
        LoadAds();
    }

    public void GetActionPointReward(Reward reward)
    {
        Debug.Log("행동력 획득함");

        CurrenyManager.Instance.actionPoint += 20; //  
        DayManager.Instance.actionPointAd--; // 한개 깍음

        DataManager.Instance.JsonSave(); // 바아로 저장 
        UIManager.instance.AsycCurrenyUI();

        LoadAds();
    }
}

 


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

    //public bool isTestMode;
    //public Text LogText;
    public Button RewardAdsBtn; // 광고 보기 버튼. UI쪽으로 옮겨야 할 듯 

    const string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // 테스트 광고 ID


    void Start()
    {
        // Google Mobile Ads SDK init 
        MobileAds.Initialize((InitializationStatus initStatus) => {}); // 초기화 

        var requestConfiguration = new RequestConfiguration.Builder()
           .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        Debug.Log("광고 준비 ");
        InitAds();
    }

    void Update()
    {
        RewardAdsBtn.interactable = rewardedAd.CanShowAd();
    }

    //광고 초기화 함수
    public void InitAds()
    {
        RewardedAd.Load(adUnitId, new AdRequest.Builder().Build(), LoadCallback);
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
    public void ShowAds()
    {
        if (rewardedAd.CanShowAd())
        {
            rewardedAd.Show(GetReward);
            Debug.Log("광고 보여줌 ");
        }
        else
        {
            Debug.Log("광고 재생 실패");
        }
    }

    //보상 함수
    public void GetReward(Reward reward)
    {
        Debug.Log("골드 획득함");

        
        CurrenyManager.Instance.gold += 100; // 일단 100골드 줘 봄 
        CurrenyManager.Instance.goldAd--; // 한개 깍음
        if(CurrenyManager.Instance.goldAd==0)
        {
            RewardAdsBtn.interactable = false; // 버튼 끈다! 
        }

        DataManager.Instance.JsonSave(); // 바아로 저장 
        DataManager.Instance.JsonLoad(); // 여기 UI init 있으므로 

        InitAds();
    }
}

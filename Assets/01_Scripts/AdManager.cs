using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // text �׽�Ʈ ���� 
//�ֵ�� ����
using GoogleMobileAds;
using GoogleMobileAds.Api; 

public class AdManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static AdManager instance;  // Singleton instance
    void Awake() // SingleTon
    {
        // �̹� �ν��Ͻ��� �����ϸ鼭 �̰� �ƴϸ� �ı� ��ȯ
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
    public Button RewardAdsBtn; // ���� ���� ��ư. UI������ �Űܾ� �� �� 

    const string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // �׽�Ʈ ���� ID


    void Start()
    {
        // Google Mobile Ads SDK init 
        MobileAds.Initialize((InitializationStatus initStatus) => {}); // �ʱ�ȭ 

        var requestConfiguration = new RequestConfiguration.Builder()
           .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        Debug.Log("���� �غ� ");
        InitAds();
    }

    void Update()
    {
        RewardAdsBtn.interactable = rewardedAd.CanShowAd();
    }

    //���� �ʱ�ȭ �Լ�
    public void InitAds()
    {
        RewardedAd.Load(adUnitId, new AdRequest.Builder().Build(), LoadCallback);
        Debug.Log("���� init �� "); 
    }


    //�ε� �ݹ� �Լ�
    public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    {
        if (rewardedAd != null)
        {
            this.rewardedAd = rewardedAd;
            Debug.Log("�ε强��");
        }
        else
        {
            Debug.Log(loadAdError.GetMessage());
        }

    }

    //���� �����ִ� �Լ�
    public void ShowAds(int rewardType)
    {
        if (rewardedAd.CanShowAd() && rewardType == 0 && CurrenyManager.Instance.goldAd > 0)
        {
            rewardedAd.Show(GetGoldReward);
            Debug.Log("���� ������ ");
        }
        else if (rewardedAd.CanShowAd() && rewardType == 1 && CurrenyManager.Instance.jellyStoneAd > 0)
        {
            rewardedAd.Show(GetJellyReward);
            Debug.Log("���� ������ ");
        }
        else
        {
            Debug.Log("���� ��� ����");
        }
    }

    //���� �Լ�
    public void GetGoldReward(Reward reward)
    {
        Debug.Log("��� ȹ����");

        
        CurrenyManager.Instance.gold += 100; // �ϴ� 100��� �� �� 
        CurrenyManager.Instance.goldAd--; // �Ѱ� ����

        DataManager.Instance.JsonSave(); // �پƷ� ���� 
        DataManager.Instance.JsonLoad(); // ���� UI init �����Ƿ� 

        InitAds();
    }

    //���� �Լ�
    public void GetJellyReward(Reward reward)
    {
        Debug.Log("���� ȹ����");


        CurrenyManager.Instance.jellyStone += 100; // �ϴ� 100���� �� �� 
        CurrenyManager.Instance.jellyStoneAd--; // �Ѱ� ����

        DataManager.Instance.JsonSave(); // �پƷ� ���� 
        DataManager.Instance.JsonLoad(); // ���� UI init �����Ƿ� 

        InitAds();
    }
}
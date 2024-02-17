
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
    const string videoAd = "ca-app-pub-9333309559865878/9920625966"; // ��¥ ���� ID
    const string videoAdTest = "ca-app-pub-3940256099942544/5224354917";

    const string goldAdId = "ca-app-pub-9333309559865878/2838210859"; // �׽�Ʈ ���� ID
    //const string jellyAdId = "ca-app-pub-9333309559865878/3299614974"; // �׽�Ʈ ���� ID


    void Start()
    {
        // Google Mobile Ads SDK init 
        MobileAds.Initialize((InitializationStatus initStatus) => {}); // �ʱ�ȭ 

        var requestConfiguration = new RequestConfiguration.Builder()
           .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        Debug.Log("���� �غ� ");
        LoadAds();

    }


    //���� �ʱ�ȭ �Լ�
    public void LoadAds()
    {
        RewardedAd.Load(videoAdTest, new AdRequest.Builder().Build(), LoadCallback);
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
        if (rewardedAd.CanShowAd() && rewardType == 0 && DayManager.Instance.goldAd > 0)
        {
            rewardedAd.Show(GetGoldReward);
            Debug.Log("���� ������ ");
        }
        else if (rewardedAd.CanShowAd() && rewardType == 1 && DayManager.Instance.jellyStoneAd > 0)
        {
            rewardedAd.Show(GetJellyReward);
            Debug.Log("���� ������ ");
        }
        else if (rewardedAd.CanShowAd() && rewardType == 2 && DayManager.Instance.actionPointAd > 0)
        {
            rewardedAd.Show(GetActionPointReward);
            Debug.Log("���� ������ ");
        }
        else
        {
            Debug.Log("���� ��� ����");
        }
    }

    public void GetGoldReward(Reward reward)
    {
        Debug.Log("��� ȹ����");

        CurrenyManager.Instance.gold += 100; // �ϴ� 100��� �� �� 
        DayManager.Instance.goldAd--; // �Ѱ� ����

        DataManager.Instance.JsonSave(); // �پƷ� ���� 
        UIManager.instance.AsycCurrenyUI();

        LoadAds();
    }

    public void GetJellyReward(Reward reward)
    {
        Debug.Log("���� ȹ����");

        CurrenyManager.Instance.jellyStone += 100; // �ϴ� 100��� �� �� 
        DayManager.Instance.jellyStoneAd--; // �Ѱ� ����

        DataManager.Instance.JsonSave(); // �پƷ� ���� 
        UIManager.instance.AsycCurrenyUI();
        LoadAds();
    }

    public void GetActionPointReward(Reward reward)
    {
        Debug.Log("�ൿ�� ȹ����");

        CurrenyManager.Instance.actionPoint += 20; //  
        DayManager.Instance.actionPointAd--; // �Ѱ� ����

        DataManager.Instance.JsonSave(); // �پƷ� ���� 
        UIManager.instance.AsycCurrenyUI();

        LoadAds();
    }
}

 


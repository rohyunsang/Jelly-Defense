using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    private DateTime lastCheckedTime = DateTime.Now;
    public bool getDailyGift = false;
    public GameObject[] dailyObjects;
    public int currentGiftDay = 0;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        DateTime currentTime = DateTime.Now;
        if (currentTime.Day != lastCheckedTime.Day)
        {
            ResetDailyActivities();
            lastCheckedTime = currentTime; // �ð� ������Ʈ
        }
    }

    // ��¥ ������ üũ�ϰ� �ʿ��� ó�� ����
    public void CheckDateChange(DateTime lastLoginDate)
    {
        DateTime currentDate = DateTime.Now.Date;

        if (lastLoginDate.Date < currentDate)
        {
            // ��¥�� ����Ǿ��ٸ�, ���� ��û ��ȸ �� �⼮üũ ���� ����
            ResetDailyActivities();
        }
    }

    // ���� Ȱ�� �缳�� (���� ��û ��ȸ, �⼮üũ ��)
    private void ResetDailyActivities()
    {
        // ����: ���� ���� ��û ��ȸ �缳��
        CurrenyManager.Instance.goldAd = 2;
        CurrenyManager.Instance.jellyStoneAd = 2;

        // ����: �⼮üũ ���� ����
        getDailyGift = false;

        // ���� ���� ����
        DataManager.Instance.JsonSave();

        // �ʿ��� UI ������Ʈ �Ǵ� �˸� ����
    }

    public void InitGiftCheck()
    {
        for(int i = 0; i < currentGiftDay; i++)
        {
            var checkImage = dailyObjects[i].transform.Find("CheckImage");
            if(checkImage != null)
            {
                checkImage.gameObject.SetActive(true);
            }
        }
    }

    public void GetDailyGiftButton()
    {
        if (!getDailyGift && currentGiftDay < dailyObjects.Length)
        {
            // "CheckImage" �ڽ� ������Ʈ�� Ȱ��ȭ�մϴ�.
            var checkImage = dailyObjects[currentGiftDay].transform.Find("CheckImage");
            if (checkImage != null)
            {
                checkImage.gameObject.SetActive(true);
            }

            getDailyGift = true; // ���� ���� ���� ������Ʈ
            currentGiftDay++; // ���� �������� �̵�
            GetDailyGiftSwitch();
            // ���� ����
            DataManager.Instance.JsonSave();
        }

        
    }

    private void GetDailyGiftSwitch()
    {
        if(currentGiftDay == 0)
        {
            CurrenyManager.Instance.gold += 3000;
            UIManager.instance.AsycCurrenyUI();
        }
        else if (currentGiftDay == 1)
        {
            CurrenyManager.Instance.gold += 3000;
            UIManager.instance.AsycCurrenyUI();
        }
        else if (currentGiftDay == 2)
        {
            CurrenyManager.Instance.gold += 3000;
            UIManager.instance.AsycCurrenyUI();
        }
        else if (currentGiftDay == 3)
        {
            CurrenyManager.Instance.gold += 3000;
            UIManager.instance.AsycCurrenyUI();
        }
        else if (currentGiftDay == 4)
        {
            CurrenyManager.Instance.gold += 3000;
            UIManager.instance.AsycCurrenyUI();
        }
        else if (currentGiftDay == 5)
        {
            CurrenyManager.Instance.gold += 3000;
            UIManager.instance.AsycCurrenyUI();
        }
        else if (currentGiftDay == 6)
        {
            CurrenyManager.Instance.gold += 3000;
            UIManager.instance.AsycCurrenyUI();
        }
    }
}

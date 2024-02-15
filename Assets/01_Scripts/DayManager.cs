using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    private DateTime lastCheckedTime = DateTime.Now;

    public static DayManager Instance { get; private set; }

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
        // Note: ���� �⼮üũ ������ ������ �䱸���׿� ���� �޶��� �� �ֽ��ϴ�.

        // ���� ���� ����
        DataManager.Instance.JsonSave();

        // �ʿ��� UI ������Ʈ �Ǵ� �˸� ����
    }
}

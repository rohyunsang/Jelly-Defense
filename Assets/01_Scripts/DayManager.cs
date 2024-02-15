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
            lastCheckedTime = currentTime; // 시간 업데이트
        }
    }

    // 날짜 변경을 체크하고 필요한 처리 수행
    public void CheckDateChange(DateTime lastLoginDate)
    {
        DateTime currentDate = DateTime.Now.Date;

        if (lastLoginDate.Date < currentDate)
        {
            // 날짜가 변경되었다면, 광고 시청 기회 및 출석체크 로직 수행
            ResetDailyActivities();
        }
    }

    // 일일 활동 재설정 (광고 시청 기회, 출석체크 등)
    private void ResetDailyActivities()
    {
        // 예시: 일일 광고 시청 기회 재설정
        CurrenyManager.Instance.goldAd = 2;
        CurrenyManager.Instance.jellyStoneAd = 2;

        // 예시: 출석체크 로직 수행
        // Note: 실제 출석체크 로직은 게임의 요구사항에 따라 달라질 수 있습니다.

        // 변경 사항 저장
        DataManager.Instance.JsonSave();

        // 필요한 UI 업데이트 또는 알림 로직
    }
}

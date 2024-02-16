using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    private DateTime lastCheckedTime = DateTime.Now;
    public bool getDailyGift = false;
    public GameObject[] dailyObjects;
    public int currentGiftDay = 0;

    public int currentGoldRefresh = 2;
    // 광고 갯수 제한 
    public int goldAd = 1;
    public int jellyStoneAd = 1;
    public int actionPointAd = 1;


    // 시간 초기화 관련 UI들 
    public TextMeshProUGUI goldRefreshInfoText;


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

    void Start()
    {
        ResetDailyActivitiesCallBackUI();
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
        goldAd = 1;
        jellyStoneAd = 1;
        actionPointAd = 1;
        currentGoldRefresh = 2;

        // 예시: 출석체크 로직 수행
        getDailyGift = false;

        // 변경 사항 저장
        DataManager.Instance.JsonSave();

        // 필요한 UI 업데이트 또는 알림 로직
        ResetDailyActivitiesCallBackUI();
    }

    private void ResetDailyActivitiesCallBackUI()
    {
        AsyncGoldRefreshText();
    }

    public void AsyncGoldRefreshText()
    {
        goldRefreshInfoText.text = "일일 남은 횟수 : " + currentGoldRefresh + "회";
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
            // "CheckImage" 자식 오브젝트를 활성화합니다.
            var checkImage = dailyObjects[currentGiftDay].transform.Find("CheckImage");
            if (checkImage != null)
            {
                checkImage.gameObject.SetActive(true);
            }

            getDailyGift = true; // 보상 수령 상태 업데이트
            currentGiftDay++; // 다음 보상으로 이동
            GetDailyGiftSwitch();
            // 상태 저장
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

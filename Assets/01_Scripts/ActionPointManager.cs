using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointManager : MonoBehaviour
{
    private const int MaxActionPoints = 180;
    private const int ActionPointsPerMinute = 3;

    #region SingleTon Pattern
    public static ActionPointManager Instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    private float lastUpdateTime = 0f; // 마지막 업데이트 시간을 추적

    private void Update()
    {
        if (Time.time - lastUpdateTime >= 60) // 60초가 지났는지 확인
        {
            IncreaseActionPoints(ActionPointsPerMinute); // 행동력 증가
            lastUpdateTime = Time.time; // 마지막 업데이트 시간 갱신
        }
    }

    public void UpdateActionPoints(DateTime lastLoginDate)
    {
            TimeSpan timeSinceLastLogin = DateTime.Now - lastLoginDate;
            int minutesSinceLastLogin = (int)timeSinceLastLogin.TotalMinutes;
            int actionPointsToAdd = minutesSinceLastLogin * ActionPointsPerMinute;

            IncreaseActionPoints(actionPointsToAdd);

            // 마지막 로그인 시간 업데이트 및 저장
            DataManager.Instance.JsonSave();
    }

    private void IncreaseActionPoints(int points)
    {
        CurrenyManager.Instance.actionPoint += points;
        if (CurrenyManager.Instance.actionPoint > MaxActionPoints)
        {
            CurrenyManager.Instance.actionPoint = MaxActionPoints;
        }
        UIManager.instance.AsycCurrenyUI();
        DataManager.Instance.JsonSave();
    }
}

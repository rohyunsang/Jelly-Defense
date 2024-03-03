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

    private float lastUpdateTime = 0f; // ������ ������Ʈ �ð��� ����

    private void Update()
    {
        if (Time.time - lastUpdateTime >= 60) // 60�ʰ� �������� Ȯ��
        {
            IncreaseActionPoints(ActionPointsPerMinute); // �ൿ�� ����
            lastUpdateTime = Time.time; // ������ ������Ʈ �ð� ����
        }
    }

    public void UpdateActionPoints(DateTime lastLoginDate)
    {
            TimeSpan timeSinceLastLogin = DateTime.Now - lastLoginDate;
            int minutesSinceLastLogin = (int)timeSinceLastLogin.TotalMinutes;
            int actionPointsToAdd = minutesSinceLastLogin * ActionPointsPerMinute;

            IncreaseActionPoints(actionPointsToAdd);

            // ������ �α��� �ð� ������Ʈ �� ����
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

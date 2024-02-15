using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    public TMP_Text currentTimerText_TMP;
    private DateTime nextRefreshTime;
    private const int refreshIntervalHours = 3; // 상점 새로고침 간격 (시간)
    private const int slimesToShow = 4; // 상점에 보여질 슬라임 수

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeNextRefreshTime();
    }

    private void Start()
    {
        UpdateShopDisplay();
    }

    private void Update()
    {
        if (DateTime.Now >= nextRefreshTime)
        {
            RefreshShop();
        }
        else
        {
            UpdateTimerDisplay();
        }
    }

    private void InitializeNextRefreshTime()
    {
        nextRefreshTime = DateTime.Now.AddHours(refreshIntervalHours);
    }

    private void RefreshShop()
    {
        // 상점 새로고침 로직
        // 예: SlimeManager에서 랜덤으로 4마리 슬라임 선택
        SlimeManager.instance.SelectRandomSlimesToShowInShop(slimesToShow);

        // 다음 새로고침 시간 설정
        InitializeNextRefreshTime();

        // 상점 UI 업데이트
        UpdateShopDisplay();
    }

    private void UpdateShopDisplay()
    {
        // 상점에 슬라임 표시 로직
        // 예: 선택된 슬라임을 상점에 표시
    }

    private void UpdateTimerDisplay()
    {
        TimeSpan timeLeft = nextRefreshTime - DateTime.Now;
        currentTimerText_TMP.text = $"{timeLeft.Hours:D2}:{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}";
    }
}

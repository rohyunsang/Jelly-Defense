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
    private const int refreshIntervalHours = 3; // ���� ���ΰ�ħ ���� (�ð�)
    private const int slimesToShow = 4; // ������ ������ ������ ��

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
        // ���� ���ΰ�ħ ����
        // ��: SlimeManager���� �������� 4���� ������ ����
        SlimeManager.instance.SelectRandomSlimesToShowInShop(slimesToShow);

        // ���� ���ΰ�ħ �ð� ����
        InitializeNextRefreshTime();

        // ���� UI ������Ʈ
        UpdateShopDisplay();
    }

    private void UpdateShopDisplay()
    {
        // ������ ������ ǥ�� ����
        // ��: ���õ� �������� ������ ǥ��
    }

    private void UpdateTimerDisplay()
    {
        TimeSpan timeLeft = nextRefreshTime - DateTime.Now;
        currentTimerText_TMP.text = $"{timeLeft.Hours:D2}:{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}";
    }
}

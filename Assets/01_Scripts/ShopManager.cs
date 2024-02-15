using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    public TMP_Text currentTimerText_TMP;
    private DateTime nextRefreshTime;
    private const int refreshIntervalHours = 3; // 상점 새로고침 간격 (시간)

    public GameObject purchasePanel;
    public TextMeshProUGUI goldPriceText;
    public TextMeshProUGUI jellyPriceText;
    public TextMeshProUGUI purchaseInfoText;
    public GameObject goldBuyButton;
    public GameObject jellyBuyButton;


    public int goldPrice;
    public int jellyPrice;

    public PurchaseOption purchaseOption;
    public string currentSlimeName;

    public GameObject purchaseSuccessPanel;
    public GameObject purchaseFailPanel;

    public void OnPurchasePanel()
    {
        goldBuyButton.SetActive(false);
        jellyBuyButton.SetActive(false);

        if (purchaseOption == PurchaseOption.JellyOnly)
        {
            jellyBuyButton.SetActive(true);
            purchaseInfoText.text = "정말로 구매하시겠습니까?";
            jellyPriceText.text = jellyPrice.ToString("N0");
        }
        else if(purchaseOption == PurchaseOption.GoldOnly)
        {
            goldBuyButton.SetActive(true);
            purchaseInfoText.text = "정말로 구매하시겠습니까?";
            goldPriceText.text = goldPrice.ToString("N0");
        }
        else if(purchaseOption == PurchaseOption.Both)
        {
            jellyBuyButton.SetActive(true);
            goldBuyButton.SetActive(true);

            purchaseInfoText.text = "골드로 구매하시겠습니까? \r\n젤리석으로 구매하시겠습니까?";
            jellyPriceText.text = jellyPrice.ToString("N0");
            goldPriceText.text = goldPrice.ToString("N0");

        }
    }

    public void GoldBuyButton()
    {
        purchasePanel.SetActive(false);
        if(CurrenyManager.Instance.gold - goldPrice >= 0)
        {
            purchaseSuccessPanel.SetActive(true);
            CurrenyManager.Instance.gold -= goldPrice;
            SlimeManager.instance.UpdateSlime(currentSlimeName);
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave();
        }
        else
        {
            purchaseFailPanel.SetActive(true);
        }
    }
    public void JellyBuyButton()
    {
        purchasePanel.SetActive(false);

        if(CurrenyManager.Instance.jellyStone - jellyPrice >= 0)
        {
            purchaseSuccessPanel.SetActive(true);
            CurrenyManager.Instance.jellyStone -= jellyPrice;
            SlimeManager.instance.UpdateSlime(currentSlimeName);
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave();
        }
        else
        {
            purchaseFailPanel.SetActive(true);
        }
    }


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
        SlimeManager.instance.RefreshShopSlimes();

        // 다음 새로고침 시간 설정
        InitializeNextRefreshTime();

    }

    private void UpdateTimerDisplay()
    {
        TimeSpan timeLeft = nextRefreshTime - DateTime.Now;
        currentTimerText_TMP.text = $"{timeLeft.Hours:D2}:{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}";
    }
}

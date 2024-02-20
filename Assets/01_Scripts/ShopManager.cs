using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    public TMP_Text currentTimerText_TMP;
    private DateTime nextRefreshTime;


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
            AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_PurchaseSound);
            purchaseSuccessPanel.SetActive(true);
            CurrenyManager.Instance.gold -= goldPrice;
            SlimeManager.instance.UpdateSlime(currentSlimeName);
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave();

            foreach (Transform child in SlimeManager.instance.shopSlimeParent)
            {
                if (child.name.Contains(currentSlimeName))
                {
                    // purchasedSlime 찾기
                    Transform purchasedSlime = child;

                    purchasedSlime.gameObject.GetComponent<Button>().enabled = false;

                    // "PurchasedCompleteImage" 자식 오브젝트 찾기
                    Transform purchasedCompleteImage = purchasedSlime.Find("PurchaseCompleteImage");
                    if (purchasedCompleteImage != null)
                    {
                        purchasedCompleteImage.gameObject.SetActive(true);
                    }


                    break; // 찾았으니 루프 종료
                }
            }
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
            AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_PurchaseSound);
            purchaseSuccessPanel.SetActive(true);
            CurrenyManager.Instance.jellyStone -= jellyPrice;
            SlimeManager.instance.UpdateSlime(currentSlimeName);
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave();

            foreach (Transform child in SlimeManager.instance.shopSlimeParent)
            {
                if (child.name.Contains(currentSlimeName))
                {
                    // purchasedSlime 찾기
                    Transform purchasedSlime = child;

                    purchasedSlime.gameObject.GetComponent<Button>().enabled = false;

                    // "PurchasedCompleteImage" 자식 오브젝트 찾기
                    Transform purchasedCompleteImage = purchasedSlime.Find("PurchaseCompleteImage");
                    if (purchasedCompleteImage != null)
                    {
                        purchasedCompleteImage.gameObject.SetActive(true);
                    }

                    break; // 찾았으니 루프 종료
                }
            }
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

    void Update()
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
        DateTime now = DateTime.Now;
        DateTime[] refreshTimes = new DateTime[]
        {
            new DateTime(now.Year, now.Month, now.Day, 3, 0, 0),
            new DateTime(now.Year, now.Month, now.Day, 6, 0, 0),
            new DateTime(now.Year, now.Month, now.Day, 9, 0, 0),
            new DateTime(now.Year, now.Month, now.Day, 12, 0, 0),
            new DateTime(now.Year, now.Month, now.Day, 15, 0, 0),
            new DateTime(now.Year, now.Month, now.Day, 18, 0, 0),
            new DateTime(now.Year, now.Month, now.Day, 21, 0, 0),
            new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1), // 다음 날의 00:00
        };

        foreach (DateTime refreshTime in refreshTimes)
        {
            if (now < refreshTime)
            {
                nextRefreshTime = refreshTime;
                return;
            }
        }

        // 현재 시간이 마지막 새로고침 시간(21:00) 이후인 경우, 다음 날의 03:00을 다음 새로고침 시간으로 설정
        nextRefreshTime = new DateTime(now.Year, now.Month, now.Day, 3, 0, 0).AddDays(1);
    }

    private void RefreshShop()
    {
        // 상점 새로고침 로직...
        InitializeNextRefreshTime();
        SlimeManager.instance.RefreshShopSlimes();
    }

    private void UpdateTimerDisplay()
    {
        TimeSpan timeLeft = nextRefreshTime - DateTime.Now;
        currentTimerText_TMP.text = string.Format("자동 갱신까지 {0:D2}:{1:D2}:{2:D2}", timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds);
    }


}

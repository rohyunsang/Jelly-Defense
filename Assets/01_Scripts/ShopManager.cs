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
            purchaseInfoText.text = "������ �����Ͻðڽ��ϱ�?";
            jellyPriceText.text = jellyPrice.ToString("N0");
        }
        else if(purchaseOption == PurchaseOption.GoldOnly)
        {
            goldBuyButton.SetActive(true);
            purchaseInfoText.text = "������ �����Ͻðڽ��ϱ�?";
            goldPriceText.text = goldPrice.ToString("N0");
        }
        else if(purchaseOption == PurchaseOption.Both)
        {
            jellyBuyButton.SetActive(true);
            goldBuyButton.SetActive(true);

            purchaseInfoText.text = "���� �����Ͻðڽ��ϱ�? \r\n���������� �����Ͻðڽ��ϱ�?";
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
                    // purchasedSlime ã��
                    Transform purchasedSlime = child;

                    purchasedSlime.gameObject.GetComponent<Button>().enabled = false;

                    // "PurchasedCompleteImage" �ڽ� ������Ʈ ã��
                    Transform purchasedCompleteImage = purchasedSlime.Find("PurchaseCompleteImage");
                    if (purchasedCompleteImage != null)
                    {
                        purchasedCompleteImage.gameObject.SetActive(true);
                    }


                    break; // ã������ ���� ����
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
                    // purchasedSlime ã��
                    Transform purchasedSlime = child;

                    purchasedSlime.gameObject.GetComponent<Button>().enabled = false;

                    // "PurchasedCompleteImage" �ڽ� ������Ʈ ã��
                    Transform purchasedCompleteImage = purchasedSlime.Find("PurchaseCompleteImage");
                    if (purchasedCompleteImage != null)
                    {
                        purchasedCompleteImage.gameObject.SetActive(true);
                    }

                    break; // ã������ ���� ����
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
            new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1), // ���� ���� 00:00
        };

        foreach (DateTime refreshTime in refreshTimes)
        {
            if (now < refreshTime)
            {
                nextRefreshTime = refreshTime;
                return;
            }
        }

        // ���� �ð��� ������ ���ΰ�ħ �ð�(21:00) ������ ���, ���� ���� 03:00�� ���� ���ΰ�ħ �ð����� ����
        nextRefreshTime = new DateTime(now.Year, now.Month, now.Day, 3, 0, 0).AddDays(1);
    }

    private void RefreshShop()
    {
        // ���� ���ΰ�ħ ����...
        InitializeNextRefreshTime();
        SlimeManager.instance.RefreshShopSlimes();
    }

    private void UpdateTimerDisplay()
    {
        TimeSpan timeLeft = nextRefreshTime - DateTime.Now;
        currentTimerText_TMP.text = string.Format("�ڵ� ���ű��� {0:D2}:{1:D2}:{2:D2}", timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds);
    }


}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int actionPoint;
    public int gold;
    public int jellyStone;

    public int goldAd;
    public int jellyStoneAd;

    // �������� Ŭ���� ���¸� ���� �ʵ� �߰�
    public List<string> stageNames = new List<string>();
    public List<bool> stageClearStatuses = new List<bool>();

    public List<string> slimeNames = new List<string>();
    public List<bool> hasSlime = new List<bool>();

    public List<string> showShopSlimes = new List<string>(); // ������ ���õ� ������ ����
    public string lastLoginDate; // ������ �α��� ��¥ ����

    public int currentGiftDay; // ���� ���� ���� �ϼ�
    public bool getDailyGift; // ���� ���� ���� ����
    public int currentGoldRefresh;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
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

        // Initialize other components or variables if needed
    }
    // ���� ���� ���
    public string path;

    void Start()
    {
        // �ȵ���̵峪 �ٸ� �÷����� ������ ���� ����Ͽ� persistentDataPath ���
        path = Path.Combine(Application.persistentDataPath, "gameData.json");
        JsonLoad(); // ���� ���� �� ������ �ҷ�����
    }

    public void JsonLoad()
    {
        if (!File.Exists(path))
        {
            // ������ ���� ��� �ʱⰪ ����
            CurrenyManager.Instance.actionPoint = 150; // ���� �ʱⰪ
            CurrenyManager.Instance.gold = 99999; // ���� �ʱⰪ
            CurrenyManager.Instance.jellyStone = 99999; // ���� �ʱⰪ
            DayManager.Instance.goldAd = 1 ;
            DayManager.Instance.jellyStoneAd = 1;

            StageManager.Instance.InitializeStageClearStatus();

            SlimeManager.instance.InitializeDefaultSlimes();
            SlimeManager.instance.RefreshShopSlimes();



            JsonSave(); // �ʱ� �����͸� ���Ͽ� ����
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            // �ҷ��� ������ GameManager�� �Ҵ�
            if (saveData != null)
            {
                CurrenyManager.Instance.actionPoint = saveData.actionPoint;
                CurrenyManager.Instance.gold = saveData.gold;
                CurrenyManager.Instance.jellyStone = saveData.jellyStone;

                DayManager.Instance.goldAd = saveData.goldAd;
                DayManager.Instance.jellyStoneAd = saveData.jellyStoneAd;


                // �������� Ŭ���� ���� �ҷ�����
                StageManager.Instance.stageClearStatus.Clear(); // ��ųʸ� �ʱ�ȭ
                for (int i = 0; i < saveData.stageNames.Count; i++)
                {
                    // �̹� �����ϴ� Ű�� ���� ó���� �ʿ� �����Ƿ�, Add ��� �ε����� ����Ͽ� ���� �Ҵ�
                    StageManager.Instance.stageClearStatus[saveData.stageNames[i]] = saveData.stageClearStatuses[i];
                }

                // ������ ���� �ҷ�����
                SlimeManager.instance.hasSlimes.Clear();
                for (int i = 0; i < saveData.slimeNames.Count; i++)
                {
                    SlimeManager.instance.hasSlimes.Add(saveData.slimeNames[i], saveData.hasSlime[i]);
                }

                SlimeManager.instance.InitStartSlimeManager();

                SlimeManager.instance.LoadShopSlime(saveData);

                // DayManager ���� �ε�
                DayManager.Instance.currentGiftDay = saveData.currentGiftDay;
                DayManager.Instance.getDailyGift = saveData.getDailyGift;
                DayManager.Instance.InitGiftCheck();
                DayManager.Instance.currentGoldRefresh = saveData.currentGoldRefresh;
                DayManager.Instance.AsyncGoldRefreshText();
            }

            // ������ �α��� ��¥ �ε� �� DayManager�� ó�� ����
            if (saveData != null && !string.IsNullOrEmpty(saveData.lastLoginDate))
            {
                DayManager.Instance.CheckDateChange(DateTime.Parse(saveData.lastLoginDate));
            }
        }
        
        // UI ���� 
        UIManager.instance.AsycCurrenyUI();
        UIManager.instance.InitAdUI(DayManager.Instance.goldAd, DayManager.Instance.jellyStoneAd); // ���� ������ ���� 
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData
        {
            actionPoint = CurrenyManager.Instance.actionPoint,
            gold = CurrenyManager.Instance.gold,
            jellyStone = CurrenyManager.Instance.jellyStone,

            goldAd = DayManager.Instance.goldAd,
            jellyStoneAd = DayManager.Instance.jellyStoneAd,
            currentGiftDay = DayManager.Instance.currentGiftDay,
            getDailyGift = DayManager.Instance.getDailyGift,
            currentGoldRefresh = DayManager.Instance.currentGoldRefresh
        };

        if (StageManager.Instance != null)
        {
            foreach (var stage in StageManager.Instance.stageClearStatus)
            {
                saveData.stageNames.Add(stage.Key);
                saveData.stageClearStatuses.Add(stage.Value);
            }
        }

        // ������ ���� ����
        saveData.slimeNames.Clear();
        saveData.hasSlime.Clear();
        foreach (var slime in SlimeManager.instance.hasSlimes)
        {
            saveData.slimeNames.Add(slime.Key);
            saveData.hasSlime.Add(slime.Value);
        }

        foreach (var slime in SlimeManager.instance.showShopSlimes)
        {
            saveData.showShopSlimes.Add(slime);
        }

        saveData.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd");

        

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);

    }
   
}
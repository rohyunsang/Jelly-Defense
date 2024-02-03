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
    private string path;

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
            CurrenyManager.Instance.gold = 1000; // ���� �ʱⰪ
            CurrenyManager.Instance.jellyStone = 1000; // ���� �ʱⰪ
            CurrenyManager.Instance.goldAd = 5 ;
            CurrenyManager.Instance.jellyStoneAd = 5;
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

                CurrenyManager.Instance.goldAd = saveData.goldAd;
                CurrenyManager.Instance.jellyStoneAd = saveData.jellyStoneAd;
            }
        }
        
        // UI ���� 
        UIManager.instance.InitCurrenyUI(CurrenyManager.Instance.actionPoint, CurrenyManager.Instance.gold, CurrenyManager.Instance.jellyStone);
        UIManager.instance.InitAdUI(CurrenyManager.Instance.goldAd, CurrenyManager.Instance.jellyStoneAd); // ���� ������ ���� 
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData
        {
            actionPoint = CurrenyManager.Instance.actionPoint,
            gold = CurrenyManager.Instance.gold,
            jellyStone = CurrenyManager.Instance.jellyStone,

            goldAd = CurrenyManager.Instance.goldAd,
            jellyStoneAd = CurrenyManager.Instance.jellyStoneAd
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
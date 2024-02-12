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
            StageManager.Instance.InitializeStageClearStatus();
            SlimeManager.instance.InitializeDefaultSlimes();
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

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);

    }
}
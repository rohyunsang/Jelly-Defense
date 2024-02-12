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

    // 스테이지 클리어 상태를 위한 필드 추가
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
    // 저장 파일 경로
    private string path;

    void Start()
    {
        // 안드로이드나 다른 플랫폼에 배포할 때를 고려하여 persistentDataPath 사용
        path = Path.Combine(Application.persistentDataPath, "gameData.json");
        JsonLoad(); // 게임 시작 시 데이터 불러오기
    }

    public void JsonLoad()
    {
        if (!File.Exists(path))
        {
            // 파일이 없을 경우 초기값 설정
            CurrenyManager.Instance.actionPoint = 150; // 예시 초기값
            CurrenyManager.Instance.gold = 1000; // 예시 초기값
            CurrenyManager.Instance.jellyStone = 1000; // 예시 초기값
            CurrenyManager.Instance.goldAd = 5 ;
            CurrenyManager.Instance.jellyStoneAd = 5;
            StageManager.Instance.InitializeStageClearStatus();
            SlimeManager.instance.InitializeDefaultSlimes();
            JsonSave(); // 초기 데이터를 파일에 저장
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            // 불러온 데이터 GameManager에 할당
            if (saveData != null)
            {
                CurrenyManager.Instance.actionPoint = saveData.actionPoint;
                CurrenyManager.Instance.gold = saveData.gold;
                CurrenyManager.Instance.jellyStone = saveData.jellyStone;

                CurrenyManager.Instance.goldAd = saveData.goldAd;
                CurrenyManager.Instance.jellyStoneAd = saveData.jellyStoneAd;


                // 스테이지 클리어 상태 불러오기
                StageManager.Instance.stageClearStatus.Clear(); // 딕셔너리 초기화
                for (int i = 0; i < saveData.stageNames.Count; i++)
                {
                    // 이미 존재하는 키에 대한 처리가 필요 없으므로, Add 대신 인덱싱을 사용하여 값을 할당
                    StageManager.Instance.stageClearStatus[saveData.stageNames[i]] = saveData.stageClearStatuses[i];
                }

                // 슬라임 정보 불러오기
                SlimeManager.instance.hasSlimes.Clear();
                for (int i = 0; i < saveData.slimeNames.Count; i++)
                {
                    SlimeManager.instance.hasSlimes.Add(saveData.slimeNames[i], saveData.hasSlime[i]);
                }
            }
        }
        
        // UI 연결 
        UIManager.instance.InitCurrenyUI(CurrenyManager.Instance.actionPoint, CurrenyManager.Instance.gold, CurrenyManager.Instance.jellyStone);
        UIManager.instance.InitAdUI(CurrenyManager.Instance.goldAd, CurrenyManager.Instance.jellyStoneAd); // 광고 갯수도 업댓 
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

        // 슬라임 정보 저장
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
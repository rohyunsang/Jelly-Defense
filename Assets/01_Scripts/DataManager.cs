using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public int actionPoint;
    public int gold;
    public int jellyStone;

    public int goldAd;
    public int jellyStoneAd;
    public int actionPointAd;

    // 스테이지 클리어 상태를 위한 필드 추가
    public List<string> stageNames = new List<string>();
    public List<bool> stageClearStatuses = new List<bool>();

    // 스테이지 별
    public List<int> stageStars = new List<int>();

    public List<string> slimeNames = new List<string>();
    public List<bool> hasSlime = new List<bool>();

    public List<string> showShopSlimes = new List<string>(); // 상점에 전시된 슬라임 정보
    public string lastLoginDate; // 마지막 로그인 날짜 저장

    public int currentGiftDay; // 현재 선물 받은 일수
    public bool getDailyGift; // 일일 선물 수령 여부
    public int currentGoldRefresh;

    public bool firstTutorial;
    public bool isUsedCoupon;
    public bool isAdPass;
    public bool isLimitedSalePurchase;
    public bool isUsedCoupon2;

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
    public string path;
    public bool isUsedCoupon;
    public bool isAdPass;
    public bool isLimitedSalePurchase;

    public bool isUsedCoupon2;

    public GameObject successCuponInfo;
    public GameObject successCuponInfo2;

    public TMP_InputField tmp_inputField;
    public void AcceptCoupon()
    {
        if(tmp_inputField.text == "한판만더해주세요" && !isUsedCoupon)
        {
            CurrenyManager.Instance.jellyStone += 500;
            CurrenyManager.Instance.gold += 50000;
            CurrenyManager.Instance.actionPoint += 100;
            if (CurrenyManager.Instance.actionPoint >= 180) CurrenyManager.Instance.actionPoint = 180;

            SlimeManager.instance.UpdateSlime("BearSlime");

            UIManager.instance.AsycCurrenyUI();
            isUsedCoupon = true;
            successCuponInfo.SetActive(true);
            DataManager.Instance.JsonSave(); // 바아로 저장 
        }
        if (tmp_inputField.text == "감사감사합니다" && !isUsedCoupon2)
        {
            CurrenyManager.Instance.jellyStone += 100;

            UIManager.instance.AsycCurrenyUI();
            isUsedCoupon2 = true;
            successCuponInfo2.SetActive(true);
            DataManager.Instance.JsonSave(); // 바아로 저장 
        }
    }

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
            CurrenyManager.Instance.actionPoint = 180; 
            CurrenyManager.Instance.gold = 0; 
            CurrenyManager.Instance.jellyStone = 0; 
            DayManager.Instance.goldAd = 1 ;
            DayManager.Instance.jellyStoneAd = 1;
            DayManager.Instance.actionPointAd = 1 ;

            StageManager.Instance.InitializeStageClearStatus();
            
            SlimeManager.instance.InitializeDefaultSlimes();
            SlimeManager.instance.RefreshShopSlimes();
            TutorialManager.Instance.StartTutorial(0);
            ScenarioManager.Instance.introScenarioScreen.SetActive(true);
            ScenarioManager.Instance.InitScenarioManager();
            

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

                DayManager.Instance.goldAd = saveData.goldAd;
                DayManager.Instance.jellyStoneAd = saveData.jellyStoneAd;
                DayManager.Instance.actionPointAd = saveData.actionPointAd;


                // 스테이지 클리어 상태 불러오기
                StageManager.Instance.stageClearStatus.Clear(); // 딕셔너리 초기화

                for (int i = 0; i < saveData.stageNames.Count; i++)
                {
                    // 이미 존재하는 키에 대한 처리가 필요 없으므로, Add 대신 인덱싱을 사용하여 값을 할당
                    StageManager.Instance.stageClearStatus.Add(saveData.stageNames[i], saveData.stageClearStatuses[i]);
                }
                for (int i = 0; i < saveData.stageNames.Count - 1; i++)
                {
                    if (saveData.stageClearStatuses[i] && i + 1<10)
                    {
                        UIManager.instance.stageButtons[i + 1].GetComponent<Image>().sprite = UIManager.instance.clearedStageImage;
                    }
                    if (saveData.stageClearStatuses[i] && i + 1 >= 10 && i < 19)
                    {
                        UIManager.instance.stageButtons[i + 1].GetComponent<Image>().sprite = UIManager.instance.clearedChaosStageImage;
                    }
                }

                StageManager.Instance.stageStarStatus.Clear();
                for (int i = 0; i < saveData.stageNames.Count; i++)
                {
                    // 이미 존재하는 키에 대한 처리가 필요 없으므로, Add 대신 인덱싱을 사용하여 값을 할당
                    StageManager.Instance.stageStarStatus[saveData.stageNames[i]] = saveData.stageStars[i];
                }

                // 슬라임 정보 불러오기
                SlimeManager.instance.hasSlimes.Clear();
                for (int i = 0; i < saveData.slimeNames.Count; i++)
                {
                    SlimeManager.instance.hasSlimes.Add(saveData.slimeNames[i], saveData.hasSlime[i]);
                }

                ActionPointManager.Instance.UpdateActionPoints(DateTime.Parse(saveData.lastLoginDate));

                SlimeManager.instance.InitStartSlimeManager();

                SlimeManager.instance.LoadShopSlime(saveData);

                // DayManager 상태 로드
                DayManager.Instance.currentGiftDay = saveData.currentGiftDay;
                DayManager.Instance.getDailyGift = saveData.getDailyGift;
                DayManager.Instance.InitGiftCheck();
                DayManager.Instance.currentGoldRefresh = saveData.currentGoldRefresh;
                DayManager.Instance.AsyncGoldRefreshText();

                StageManager.Instance.AsyncJsonStageStars();

                for (int i = 0; i < saveData.showShopSlimes.Count; i++)
                {
                    SlimeManager.instance.showShopSlimes.Add(saveData.showShopSlimes[i]);
                }

                TutorialManager.Instance.firstTutorial = saveData.firstTutorial;
                ScenarioManager.Instance.InitScenarioManager();
                isUsedCoupon = saveData.isUsedCoupon;
                isUsedCoupon2 = saveData.isUsedCoupon2;

                if (saveData.isAdPass == null || saveData.isAdPass == false)
                    isAdPass = false;
                else
                {
                    isAdPass = saveData.isAdPass;
                }
                if(saveData.isLimitedSalePurchase == null || saveData.isLimitedSalePurchase == false)
                {
                    isLimitedSalePurchase = false;
                }
                else
                {
                    isLimitedSalePurchase = saveData.isLimitedSalePurchase;
                }



            }

            // 마지막 로그인 날짜 로드 및 DayManager로 처리 전달
            if (saveData != null && !string.IsNullOrEmpty(saveData.lastLoginDate))
            {
                DayManager.Instance.CheckDateChange(DateTime.Parse(saveData.lastLoginDate));
            }
        }
        
        // UI 연결 
        UIManager.instance.AsycCurrenyUI();
        JsonSave();
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
            actionPointAd = DayManager.Instance.actionPointAd,
            currentGiftDay = DayManager.Instance.currentGiftDay,
            getDailyGift = DayManager.Instance.getDailyGift,
            currentGoldRefresh = DayManager.Instance.currentGoldRefresh,
            firstTutorial = TutorialManager.Instance.firstTutorial,
            isUsedCoupon = isUsedCoupon,
            isUsedCoupon2 = isUsedCoupon2,
            isAdPass = isAdPass,
            isLimitedSalePurchase = isLimitedSalePurchase
        };

        
        foreach (var stage in StageManager.Instance.stageClearStatus)
        {
            saveData.stageNames.Add(stage.Key);
            saveData.stageClearStatuses.Add(stage.Value);
        }
        foreach(var stageStar in StageManager.Instance.stageStarStatus)
        {
            saveData.stageStars.Add(stageStar.Value);
        }
        

        // 슬라임 정보 저장
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

        saveData.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // 날짜와 시간 모두 저장

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);

    }


   
}
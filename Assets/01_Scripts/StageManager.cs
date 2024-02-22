using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static StageManager Instance { get; private set; }
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
        stageClearStatus = new Dictionary<string, bool>();
        stageStarStatus = new Dictionary<string, int>();    
    }

    #endregion

    [SerializeField]
    public Dictionary<string, bool> stageClearStatus;

    [SerializeField]
    public Dictionary<string, int> stageStarStatus;

    public bool objectOpen = false;

    public List<StageRewardData> stageRewards = new List<StageRewardData>();

    private void Start()
    {
        InitializeStageRewards();
    }

    void InitializeStageRewards()
    {
        stageRewards.Clear(); // 기존 데이터가 있다면 초기화

        // 스테이지 보상 데이터 초기화
        stageRewards.Add(new StageRewardData("NormalStage1", 500, new int[] { 5, 15, 20 }));
        stageRewards.Add(new StageRewardData("NormalStage2", 600, new int[] { 5, 15, 20 }));
        stageRewards.Add(new StageRewardData("NormalStage3", 700, new int[] { 5, 15, 20 }));
        stageRewards.Add(new StageRewardData("NormalStage4", 1500, new int[] { 10, 20, 30 }));
        stageRewards.Add(new StageRewardData("NormalStage5", 1000, new int[] { 5, 15, 20 }));
        stageRewards.Add(new StageRewardData("NormalStage6", 1100, new int[] { 5, 15, 20 }));
        stageRewards.Add(new StageRewardData("NormalStage7", 2000, new int[] { 10, 20, 30 }));
        stageRewards.Add(new StageRewardData("NormalStage8", 1400, new int[] { 5, 15, 20 }));
        stageRewards.Add(new StageRewardData("NormalStage9", 1700, new int[] { 5, 15, 20 }));
        stageRewards.Add(new StageRewardData("NormalStage10", 3500, new int[] { 10, 20, 30 }));

        // Chaos 스테이지 보상 데이터 추가
        stageRewards.Add(new StageRewardData("ChaosStage1", 800, new int[] { 15, 25, 40 }));
        stageRewards.Add(new StageRewardData("ChaosStage2", 900, new int[] { 15, 25, 40 }));
        stageRewards.Add(new StageRewardData("ChaosStage3", 1000, new int[] { 15, 25, 40 }));
        stageRewards.Add(new StageRewardData("ChaosStage4", 2800, new int[] { 20, 30, 50 }));
        stageRewards.Add(new StageRewardData("ChaosStage5", 1500, new int[] { 15, 25, 40 }));
        stageRewards.Add(new StageRewardData("ChaosStage6", 1700, new int[] { 15, 25, 40 }));
        stageRewards.Add(new StageRewardData("ChaosStage7", 3800, new int[] { 20, 30, 50 }));
        stageRewards.Add(new StageRewardData("ChaosStage8", 2000, new int[] { 15, 25, 40 }));
        stageRewards.Add(new StageRewardData("ChaosStage9", 2500, new int[] { 15, 25, 40 }));
        stageRewards.Add(new StageRewardData("ChaosStage10", 5500, new int[] { 20, 30, 50 }));
    }

    public void GrantStageRewards(string stageName, int starCount)
    {
        StageRewardData rewardData = stageRewards.Find(stage => stage.stageName == stageName);
        if (rewardData != null)
        {
            // 골드 보상 항상 지급
            UIManager.instance.goldTextHUD.text = rewardData.goldReward.ToString();
            // 젤리 보상은 처음 클리어 시에만 지급
            int jellyReward = 0;
            if (!IsStageCleared(stageName) || stageStarStatus[stageName] < starCount)
            {
                if (stageStarStatus[stageName] == 10000) // 미클리어 였을때 
                {
                    if(stageName.Contains("Normal"))
                        ScenarioManager.Instance.InitScenarioStageStory(stageName);

                    if(starCount == 11000) // 1클리어
                    {
                        jellyReward += rewardData.jellyRewards[0];
                    }
                    else if (starCount == 11100 || starCount == 11010) // 2클리어
                    {
                        jellyReward += rewardData.jellyRewards[0];
                        jellyReward += rewardData.jellyRewards[1];
                    }
                    else if(starCount == 11110)
                    {
                        jellyReward += rewardData.jellyRewards[0];
                        jellyReward += rewardData.jellyRewards[1];
                        jellyReward += rewardData.jellyRewards[2];
                    }
                }
                if (stageStarStatus[stageName] == 11000) // 1클리어 였을때 
                {
                    if (starCount == 11100 || starCount == 11010) // 2클리어
                    {
                        jellyReward += rewardData.jellyRewards[1];
                    }
                    else if (starCount == 11110)
                    {
                        jellyReward += rewardData.jellyRewards[1];
                        jellyReward += rewardData.jellyRewards[2];
                    }
                }
                else if (stageStarStatus[stageName] == 11100 || stageStarStatus[stageName] == 11010) // 2클리어 였을때 
                {
                    if (starCount == 11110)
                    {
                        jellyReward += rewardData.jellyRewards[2];
                    }
                }
                
                UIManager.instance.jellyTextHUD.text = jellyReward.ToString();
                UIManager.instance.jellyImageHUD.SetActive(true);
            }
            if(starCount == 11000)
            {
                UIManager.instance.leftStar.SetActive(true);
            }else if (starCount == 11100)
            {
                UIManager.instance.leftStar.SetActive(true);
                UIManager.instance.middelStar.SetActive(true);
            }
            else if (starCount == 11010)
            {
                UIManager.instance.leftStar.SetActive(true);
                UIManager.instance.rightStar.SetActive(true);
            }
            else if (starCount == 11110)
            {
                UIManager.instance.leftStar.SetActive(true);
                UIManager.instance.middelStar.SetActive(true);
                UIManager.instance.rightStar.SetActive(true);
            }

            CurrenyManager.Instance.gold += rewardData.goldReward;
            CurrenyManager.Instance.jellyStone += jellyReward;
            UIManager.instance.AsycCurrenyUI();
        }
        else
        {
            Debug.LogError("Invalid stage name: " + stageName);
        }
    }

    public void InitializeStageClearStatus()
    {
        stageClearStatus = new Dictionary<string, bool>();
        stageStarStatus = new Dictionary<string, int>();

        // 일반모드 1-10, 카오스모드 1-10 초기화
        for (int i = 1; i <= 10; i++)
        {
            stageClearStatus.Add("NormalStage" + i, false);
        }
        for (int i = 1; i <= 10; i++)
        {
            stageClearStatus.Add("ChaosStage" + i, false);
        }
        for (int i = 1; i <= 10; i++)
        {
            stageStarStatus.Add("NormalStage" + i, 10000);
        }
        for (int i = 1; i <= 10; i++)
        {
            stageStarStatus.Add("ChaosStage" + i, 10000);
        }
    }

    public void AsyncJsonStageStars()
    {
        int idx = 0;
        foreach (var stageStar in stageStarStatus)
        {
            string stageName = stageStar.Key;
            int starCount = stageStar.Value;

            // 별의 개수에 따른 UI 업데이트 로직
            switch (starCount)
            {
                case 10000:
                    break;

                case 11000:
                    UIManager.instance.stageStars[idx * 3].SetActive(true);
                    break;

                case 11100:
                    UIManager.instance.stageStars[idx * 3].SetActive(true);
                    UIManager.instance.stageStars[idx * 3 + 1].SetActive(true);
                    break;

                case 11010:
                    UIManager.instance.stageStars[idx * 3].SetActive(true);
                    UIManager.instance.stageStars[idx * 3 + 2].SetActive(true);
                    break;

                case 11110:
                    UIManager.instance.stageStars[idx * 3].SetActive(true);
                    UIManager.instance.stageStars[idx * 3 + 1].SetActive(true);
                    UIManager.instance.stageStars[idx * 3 + 2].SetActive(true);
                    break;

                default:
                    // 예외적인 경우의 처리
                    Debug.LogError("Unexpected number of stars for " + stageName);
                    break;
            }
            idx++;
        }
    }

    public void EstimateStageStar()
    {
        int starCount = 11000; // 기본 11000 제공 클리어 했으니까.

        if (GameObject.FindWithTag("SlimeCastle").GetComponent<SlimeCastle>().currentHP >= 700f)
        {
            starCount += 100;
        }

        if(UIManager.instance.selectedStageName.Contains("4") || UIManager.instance.selectedStageName.Contains("7") || UIManager.instance.selectedStageName.Contains("10"))
        {
            starCount += 10;
        }
        else
        {
            if (objectOpen)
            {
                starCount += 10;
                objectOpen = false;
            }
        }

        GrantStageRewards(UIManager.instance.selectedStageName, starCount); // UI 부분

        SetStageCleared();
        SetStageStar(starCount);
        DataManager.Instance.JsonSave();
        AsyncJsonStageStars();

       
        
    }



    public bool CanEnterStage(string stageName)
    {
        // 카오스 스테이지 진입 조건 확인
        if (stageName.StartsWith("ChaosStage"))
        {
            // 카오스 스테이지 1 진입 조건: 일반 스테이지 10 클리어
            if (stageName == "ChaosStage1" && IsStageCleared("NormalStage10"))
            {
                return true;
            }// 카오스 스테이지 2 이상의 경우 이전 카오스 스테이지 클리어 여부 확인
            else
            {
                int stageNumber = int.Parse(stageName.Replace("ChaosStage", ""));
                if (stageNumber > 1)
                {
                    // 이전 카오스 스테이지의 이름 생성
                    string previousStageName = "ChaosStage" + (stageNumber - 1);
                    // 이전 카오스 스테이지가 클리어되었는지 확인
                    return IsStageCleared(previousStageName);
                }
            }
        }
        else if (stageName.StartsWith("NormalStage"))
        {
            int stageNumber = int.Parse(stageName.Replace("NormalStage", ""));
            // 일반 스테이지 1은 항상 진입 가능
            if (stageNumber == 1)
            {
                return true;
            }
            // 그 외의 일반 스테이지는 이전 스테이지 클리어 여부 확인
            else if (stageNumber > 1)
            {
                return IsStageCleared("NormalStage" + (stageNumber - 1));
            }
        }
        return false; // 기본적으로는 진입 불가능
    }

    // 게임 클리어 하면 이 함수를 실행.
    public void SetStageCleared()
    {
        if (stageClearStatus.ContainsKey(UIManager.instance.selectedStageName))
        {
            stageClearStatus[UIManager.instance.selectedStageName] = true;
        }
        else
        {
            Debug.LogError("Stage name does not exist: " + UIManager.instance.selectedStageName);
        }

        // 스테이지 버튼 색깔 바꾸는 거
        int index;

        if (UIManager.instance.selectedStageName.StartsWith("NormalStage"))
        {
            // "NormalStage" 뒤의 숫자를 파싱하여 인덱스로 사용
            string numberString = UIManager.instance.selectedStageName.Replace("NormalStage", "");
            int number = int.Parse(numberString);
            index = number - 1; // 숫자에서 1을 빼서 0부터 시작하도록 조정
        }
        else if (UIManager.instance.selectedStageName.StartsWith("ChaosStage"))
        {
            // "ChaosStage" 뒤의 숫자를 파싱하여 인덱스로 사용
            string numberString = UIManager.instance.selectedStageName.Replace("ChaosStage", "");
            int number = int.Parse(numberString);
            index = 10 + (number - 1); // 10을 더하고, 1을 빼서 10부터 시작하도록 조정
            UIManager.instance.stageButtons[index].GetComponent<Image>().sprite = UIManager.instance.clearedChaosStageImage;

        }
        else
        {
            // 에러 처리 또는 기본값 설정
            index = 0; // 유효하지 않은 인덱스 값
        }
        if (index < 9)  // 9스테이지까지 다음 스테이지 열음
        {
            UIManager.instance.stageButtons[index + 1].GetComponent<Image>().sprite = UIManager.instance.clearedStageImage;

        }
        else if (index <= 18) // 18이면 카오스 9스테이지 -> 카오스 10스테이지까지 
        {
            Debug.Log("카오스저자앚이ㅏㅣㅇ자ㅣ" + index);
            UIManager.instance.stageButtons[index + 1].GetComponent<Image>().sprite = UIManager.instance.clearedChaosStageImage;

        }// 노말 10이 index 9니까. 


    }

    public void SetStageStar(int starCnt)
    {
        Debug.Log("스타의 개수는@@@@@@@@@@@@@@@@@@@@@@@@@@@@" + starCnt);
        if (stageStarStatus.ContainsKey(UIManager.instance.selectedStageName))
        {
            stageStarStatus[UIManager.instance.selectedStageName] = starCnt;
        }
        else
        {
            Debug.LogError("Stage name does not exist: " + UIManager.instance.selectedStageName);
        }
    }

    public bool IsStageCleared(string stageName)
    {
        return stageClearStatus.TryGetValue(stageName, out bool cleared) && cleared;
    }
}

[System.Serializable]
public class StageRewardData
{
    public string stageName;
    public int goldReward;
    public int[] jellyRewards; // 인덱스 0: 1별, 인덱스 1: 2별, 인덱스 2: 3별

    public StageRewardData(string name, int gold, int[] jelly)
    {
        stageName = name;
        goldReward = gold;
        jellyRewards = jelly;
    }
}
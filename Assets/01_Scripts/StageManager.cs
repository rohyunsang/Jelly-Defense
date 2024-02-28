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
        stageRewards.Clear(); // ���� �����Ͱ� �ִٸ� �ʱ�ȭ

        // �������� ���� ������ �ʱ�ȭ
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

        // Chaos �������� ���� ������ �߰�
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
            // ��� ���� �׻� ����
            UIManager.instance.goldTextHUD.text = rewardData.goldReward.ToString();
            // ���� ������ ó�� Ŭ���� �ÿ��� ����
            int jellyReward = 0;
            if (!IsStageCleared(stageName) || stageStarStatus[stageName] < starCount)
            {
                if (stageStarStatus[stageName] == 10000) // ��Ŭ���� ������ 
                {
                    if(stageName.Contains("Normal"))
                        ScenarioManager.Instance.InitScenarioStageStory(stageName);

                    if(starCount == 11000) // 1Ŭ����
                    {
                        jellyReward += rewardData.jellyRewards[0];
                    }
                    else if (starCount == 11100 || starCount == 11010) // 2Ŭ����
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
                if (stageStarStatus[stageName] == 11000) // 1Ŭ���� ������ 
                {
                    if (starCount == 11100 || starCount == 11010) // 2Ŭ����
                    {
                        jellyReward += rewardData.jellyRewards[1];
                    }
                    else if (starCount == 11110)
                    {
                        jellyReward += rewardData.jellyRewards[1];
                        jellyReward += rewardData.jellyRewards[2];
                    }
                }
                else if (stageStarStatus[stageName] == 11100 || stageStarStatus[stageName] == 11010) // 2Ŭ���� ������ 
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

        // �Ϲݸ�� 1-10, ī������� 1-10 �ʱ�ȭ
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

            // ���� ������ ���� UI ������Ʈ ����
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
                    // �������� ����� ó��
                    Debug.LogError("Unexpected number of stars for " + stageName);
                    break;
            }
            idx++;
        }
    }

    public void EstimateStageStar()
    {
        int starCount = 11000; // �⺻ 11000 ���� Ŭ���� �����ϱ�.

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

        GrantStageRewards(UIManager.instance.selectedStageName, starCount); // UI �κ�

        SetStageCleared();
        SetStageStar(starCount);
        DataManager.Instance.JsonSave();
        AsyncJsonStageStars();

       
        
    }



    public bool CanEnterStage(string stageName)
    {
        // ī���� �������� ���� ���� Ȯ��
        if (stageName.StartsWith("ChaosStage"))
        {
            // ī���� �������� 1 ���� ����: �Ϲ� �������� 10 Ŭ����
            if (stageName == "ChaosStage1" && IsStageCleared("NormalStage10"))
            {
                return true;
            }// ī���� �������� 2 �̻��� ��� ���� ī���� �������� Ŭ���� ���� Ȯ��
            else
            {
                int stageNumber = int.Parse(stageName.Replace("ChaosStage", ""));
                if (stageNumber > 1)
                {
                    // ���� ī���� ���������� �̸� ����
                    string previousStageName = "ChaosStage" + (stageNumber - 1);
                    // ���� ī���� ���������� Ŭ����Ǿ����� Ȯ��
                    return IsStageCleared(previousStageName);
                }
            }
        }
        else if (stageName.StartsWith("NormalStage"))
        {
            int stageNumber = int.Parse(stageName.Replace("NormalStage", ""));
            // �Ϲ� �������� 1�� �׻� ���� ����
            if (stageNumber == 1)
            {
                return true;
            }
            // �� ���� �Ϲ� ���������� ���� �������� Ŭ���� ���� Ȯ��
            else if (stageNumber > 1)
            {
                return IsStageCleared("NormalStage" + (stageNumber - 1));
            }
        }
        return false; // �⺻�����δ� ���� �Ұ���
    }

    // ���� Ŭ���� �ϸ� �� �Լ��� ����.
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

        // �������� ��ư ���� �ٲٴ� ��
        int index;

        if (UIManager.instance.selectedStageName.StartsWith("NormalStage"))
        {
            // "NormalStage" ���� ���ڸ� �Ľ��Ͽ� �ε����� ���
            string numberString = UIManager.instance.selectedStageName.Replace("NormalStage", "");
            int number = int.Parse(numberString);
            index = number - 1; // ���ڿ��� 1�� ���� 0���� �����ϵ��� ����
        }
        else if (UIManager.instance.selectedStageName.StartsWith("ChaosStage"))
        {
            // "ChaosStage" ���� ���ڸ� �Ľ��Ͽ� �ε����� ���
            string numberString = UIManager.instance.selectedStageName.Replace("ChaosStage", "");
            int number = int.Parse(numberString);
            index = 10 + (number - 1); // 10�� ���ϰ�, 1�� ���� 10���� �����ϵ��� ����
            UIManager.instance.stageButtons[index].GetComponent<Image>().sprite = UIManager.instance.clearedChaosStageImage;

        }
        else
        {
            // ���� ó�� �Ǵ� �⺻�� ����
            index = 0; // ��ȿ���� ���� �ε��� ��
        }
        if (index < 9)  // 9������������ ���� �������� ����
        {
            UIManager.instance.stageButtons[index + 1].GetComponent<Image>().sprite = UIManager.instance.clearedStageImage;

        }
        else if (index <= 18) // 18�̸� ī���� 9�������� -> ī���� 10������������ 
        {
            Debug.Log("ī�������ڝ��̤��Ӥ��ڤ�" + index);
            UIManager.instance.stageButtons[index + 1].GetComponent<Image>().sprite = UIManager.instance.clearedChaosStageImage;

        }// �븻 10�� index 9�ϱ�. 


    }

    public void SetStageStar(int starCnt)
    {
        Debug.Log("��Ÿ�� ������@@@@@@@@@@@@@@@@@@@@@@@@@@@@" + starCnt);
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
    public int[] jellyRewards; // �ε��� 0: 1��, �ε��� 1: 2��, �ε��� 2: 3��

    public StageRewardData(string name, int gold, int[] jelly)
    {
        stageName = name;
        goldReward = gold;
        jellyRewards = jelly;
    }
}
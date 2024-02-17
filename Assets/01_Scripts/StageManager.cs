using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
            stageStarStatus.Add("NormalStage" + i, 0);
        }
        for (int i = 1; i <= 10; i++)
        {
            stageStarStatus.Add("ChaosStage" + i, 0);
        }
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
            }
            // 카오스 스테이지 2 이상의 경우 이전 카오스 스테이지 클리어 여부 확인 (여기서는 예시로 구현하지 않음)
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
            DataManager.Instance.JsonSave();
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

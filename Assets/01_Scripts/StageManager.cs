using System.Collections;
using System.Collections.Generic;
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
    }
    #endregion

    [SerializeField]
    public Dictionary<string, bool> stageClearStatus;

    private void InitializeStageClearStatus()
    {
        stageClearStatus = new Dictionary<string, bool>();

        // 일반모드 1-10, 카오스모드 1-10 초기화
        for (int i = 1; i <= 10; i++)
        {
            stageClearStatus.Add("NormalStage" + i, false);
            stageClearStatus.Add("ChaosStage" + i, false);
        }
    }

    // 게임 클리어 하면 이 함수를 실행.
    public void SetStageCleared(string stageName, bool cleared)
    {
        if (stageClearStatus.ContainsKey(stageName))
        {
            stageClearStatus[stageName] = cleared;
        }
        else
        {
            Debug.LogError("Stage name does not exist: " + stageName);
        }
    }

    public bool IsStageCleared(string stageName)
    {
        return stageClearStatus.TryGetValue(stageName, out bool cleared) && cleared;
    }
}

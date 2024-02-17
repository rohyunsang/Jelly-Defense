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
            stageStarStatus.Add("NormalStage" + i, 0);
        }
        for (int i = 1; i <= 10; i++)
        {
            stageStarStatus.Add("ChaosStage" + i, 0);
        }
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
            }
            // ī���� �������� 2 �̻��� ��� ���� ī���� �������� Ŭ���� ���� Ȯ�� (���⼭�� ���÷� �������� ����)
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

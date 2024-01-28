using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static GameManager Instance { get; private set; }
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
        InitializeStageClearStatus();
    }
    #endregion

    [SerializeField]
    public Dictionary<string, bool> stageClearStatus;

    private void InitializeStageClearStatus()
    {
        stageClearStatus = new Dictionary<string, bool>();

        // �Ϲݸ�� 1-10, ī������� 1-10 �ʱ�ȭ
        for (int i = 1; i <= 10; i++)
        {
            stageClearStatus.Add("NormalStage" + i, false);
            stageClearStatus.Add("ChaosStage" + i, false);
        }
    }

    // ���� Ŭ���� �ϸ� �� �Լ��� ����.
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

    public void ChangeSceneToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void ChangeScene(string stageName) // stage�̵��Ҷ�.
    {
        StartCoroutine(LoadSceneAndPerformAction(stageName));
    }

    private IEnumerator LoadSceneAndPerformAction(string sceneName)
    {
        // �񵿱������� �� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // �� �ε��� �Ϸ�� ������ ���
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // �� �ε� �Ϸ� �� ������ �ڵ�
        Debug.Log("Scene Loaded");
        SlimeSpawnManager.instance.FindSlimeSpawn();
        EnemySpawnManager.instance.EnemySpawnTable(sceneName);
        SlimeSpawnManager.instance.InitSlimeSpawnManager();
    }
    public void PauseGame()
    {
        Time.timeScale = 0; // �ð��� ����
    }

    // ���� �簳 �Լ�
    public void ResumeGame()
    {
        Time.timeScale = 1; // �ð��� �ٽ� ����
    }

    public void OffCamera()
    {

    }
}

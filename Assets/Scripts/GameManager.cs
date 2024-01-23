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

    public void ChangeSceneToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void ChangeScene(string stageName) // stage이동할때.
    {
        StartCoroutine(LoadSceneAndPerformAction(stageName));
    }

    private IEnumerator LoadSceneAndPerformAction(string sceneName)
    {
        // 비동기적으로 씬 로딩
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 씬 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 씬 로딩 완료 후 실행할 코드
        Debug.Log("Scene Loaded");
        SlimeSpawnManager.instance.FindSlimeSpawn();
        EnemySpawnManager.instance.EnemySpawnTable(sceneName);
        SlimeSpawnManager.instance.InitSlimeSpawnManager();
    }
    public void PauseGame()
    {
        Time.timeScale = 0; // 시간을 멈춤
    }

    // 게임 재개 함수
    public void ResumeGame()
    {
        Time.timeScale = 1; // 시간을 다시 시작
    }

    public void OffCamera()
    {

    }
}

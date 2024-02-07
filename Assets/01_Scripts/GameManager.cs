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
    }
    #endregion

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
        SlimeSpawnManager.instance.FindSlimeSpawn();
        SlimeSpawnManager.instance.InitSlimeSpawnManager();
        EnemySpawnManager.instance.EnemySpawnTable(sceneName);
        PlayerSkillManager.instance.InitPlayerSkill();
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
}

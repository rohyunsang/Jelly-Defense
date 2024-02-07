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
        SlimeSpawnManager.instance.FindSlimeSpawn();
        SlimeSpawnManager.instance.InitSlimeSpawnManager();
        EnemySpawnManager.instance.EnemySpawnTable(sceneName);
        PlayerSkillManager.instance.InitPlayerSkill();
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
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

    public bool goPickUpStage = false;

    public void ChangeSceneToMain()
    {
        StartCoroutine(LoadSceneAndPerformAction("Main"));
    }

    public void ChangeScene(string stageName) // stage�̵��Ҷ�.
    {
        if (stageName == "Stage2" || stageName == "Stage3") stageName = "Stage1";
        else if (stageName == "Stage5" || stageName == "Stage6" || stageName == "Stage7") stageName = "Stage4";
        else if (stageName == "Stage9" || stageName == "Stage10") stageName = "Stage8";
        StartCoroutine(LoadSceneAndPerformAction(stageName));
    }

    private IEnumerator LoadSceneAndPerformAction(string sceneName)
    {
        string preStageName = UIManager.instance.selectedStageName;

        // �񵿱������� �� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // �� �ε��� �Ϸ�� ������ ���
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if(sceneName == "Main")
        {
            if (goPickUpStage) {
                UIManager.instance.OnClickBattleButton();
                UIManager.instance.pickUpScreen.SetActive(true);
                UIManager.instance.selectedStageName = preStageName;
                goPickUpStage = false;
            }
            
        }
        else
        {
            SlimeSpawnManager.instance.FindSlimeSpawn();
            SlimeSpawnManager.instance.InitSlimeSpawnManager();
            EnemySpawnManager.instance.EnemySpawnTable(sceneName);
            PlayerSkillManager.instance.InitPlayerSkill();
            EnhanceObject.Instance.StageSwitch();
        }
        


    }

    public void InitAllStageEnd()
    {
        SlimeSpawnManager.instance.isEnhanced = false;
        EnemySpawnManager.instance.isEnhanced = false;

        foreach (Transform child in SlimeSpawnManager.instance.slimeParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in EnemySpawnManager.instance.enemyParent.transform)
        {
            Destroy(child.gameObject);
        }
        PlayerSkillManager.instance.StageEndSettingInit();
        OriginTimeScale();
        UIManager.instance.lockImage.SetActive(false);
        UIManager.instance.VisibleSlimeSpawnIcon();
        UIManager.instance.ResetOrder();
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

    public void DoubleTimeScale()
    {
        Time.timeScale = 1.5f;
    }
    public void OriginTimeScale()
    {
        Time.timeScale = 1;
    }
}

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

    public void ChangeScene(string stageName) // stage이동할때.
    {
        if (stageName == "Stage2" || stageName == "Stage3") stageName = "Stage1";
        else if (stageName == "Stage5" || stageName == "Stage6" || stageName == "Stage7") stageName = "Stage4";
        else if (stageName == "Stage9" || stageName == "Stage10") stageName = "Stage8";
        StartCoroutine(LoadSceneAndPerformAction(stageName));
    }

    private IEnumerator LoadSceneAndPerformAction(string sceneName)
    {
        string preStageName = UIManager.instance.selectedStageName;

        // 비동기적으로 씬 로딩
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 씬 로딩이 완료될 때까지 대기
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
        Time.timeScale = 0; // 시간을 멈춤
    }

    // 게임 재개 함수
    public void ResumeGame()
    {
        Time.timeScale = 1; // 시간을 다시 시작
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

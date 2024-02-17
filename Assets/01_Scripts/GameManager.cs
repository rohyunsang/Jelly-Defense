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
        
        StartCoroutine(LoadSceneAndPerformAction(stageName));
    }

    private IEnumerator LoadSceneAndPerformAction(string sceneName)
    {
        string preStageName = UIManager.instance.selectedStageName;
        string originScene = "";
        originScene = sceneName;
        if (sceneName == "NormalStage2" || sceneName == "NormalStage3") sceneName = "NormalStage1";
        else if (sceneName == "NormalStage5" || sceneName == "NormalStage6" || sceneName == "NormalStage7") sceneName = "NormalStage4";
        else if (sceneName == "NormalStage9" || sceneName == "NormalStage10") sceneName = "NormalStage8";

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
            EnemySpawnManager.instance.EnemySpawnTable(originScene);
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
        UIManager.instance.objectDesText.text = "";
        UIManager.instance.objectImage.SetActive(false);
        UIManager.instance.objectShining.SetActive(false);
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

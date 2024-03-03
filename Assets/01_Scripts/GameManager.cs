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
        if (sceneName == "NormalStage2" || sceneName == "NormalStage3" || sceneName == "NormalStage4") sceneName = "NormalStage1";
        else if (sceneName == "NormalStage6" || sceneName == "NormalStage7") sceneName = "NormalStage5";
        else if (sceneName == "NormalStage9" || sceneName == "NormalStage10") sceneName = "NormalStage8";

        if (sceneName == "ChaosStage2" || sceneName == "ChaosStage3" || sceneName == "ChaosStage4") sceneName = "ChaosStage1";
        else if (sceneName == "ChaosStage6" || sceneName == "ChaosStage7") sceneName = "ChaosStage5";
        else if (sceneName == "ChaosStage9" || sceneName == "ChaosStage10") sceneName = "ChaosStage8";

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
            AudioManager.Instance.PlayBgm(AudioManager.BGM.BGM_Lobby);

        }
        else
        {
            SlimeSpawnManager.instance.FindSlimeSpawn();
            SlimeSpawnManager.instance.InitSlimeSpawnManager();
            EnemySpawnManager.instance.EnemySpawnTable(originScene);
            PlayerSkillManager.instance.InitPlayerSkill();
            EnhanceObject.Instance.StageSwitch();
            
            
            CurrenyManager.Instance.actionPoint -= UIManager.instance.stageActionPoint;
            UIManager.instance.AsycCurrenyUI();
            DataManager.Instance.JsonSave();

            ChangeBackgroundMusicBasedOnScene(sceneName);
        }
    }


    private void ChangeBackgroundMusicBasedOnScene(string sceneName)
    {
        AudioManager.BGM bgmToPlay = AudioManager.BGM.BGM_Lobby; // Default BGM

        // Determine the BGM based on the sceneName
        switch (sceneName)
        {
            case "NormalStage1":
            case "NormalStage2":
            case "NormalStage3":
                bgmToPlay = AudioManager.BGM.BGM_NormalStage1;
                break;
            case "NormalStage4":
                bgmToPlay = AudioManager.BGM.BGM_NormalStage4;
                break;
            case "NormalStage5":
            case "NormalStage6":
                bgmToPlay = AudioManager.BGM.BGM_NormalStage5;
                break;
            case "NormalStage7":
                bgmToPlay = AudioManager.BGM.BGM_NormalStage7;
                break;
            case "NormalStage8":
            case "NormalStage9":
                bgmToPlay = AudioManager.BGM.BGM_NormalStage8;
                break;
            case "NormalStage10":
                bgmToPlay = AudioManager.BGM.BGM_NormalStage10;
                break;

            case "ChaosStage1":
            case "ChaosStage2":
            case "ChaosStage3":
                bgmToPlay = AudioManager.BGM.BGM_ChaosStage1;
                break;
            case "ChaosStage4":
                bgmToPlay = AudioManager.BGM.BGM_ChaosStage4;
                break;
            case "ChaosStage5":
            case "ChaosStage6":
                bgmToPlay = AudioManager.BGM.BGM_ChaosStage5;
                break;
            case "ChaosStage7":
                bgmToPlay = AudioManager.BGM.BGM_ChaosStage7;
                break;
            case "ChaosStage8":
            case "ChaosStage9":
                bgmToPlay = AudioManager.BGM.BGM_ChaosStage8;
                break;
            case "ChaosStage10":
                bgmToPlay = AudioManager.BGM.BGM_ChaosStage10;
                break;
        }

        // Use AudioManager's instance to play the selected BGM
        AudioManager.Instance.PlayBgm(bgmToPlay);
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

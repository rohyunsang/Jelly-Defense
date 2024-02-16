using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
#region SingleTon Pattern
    public static UIManager instance;  // Singleton instance
    void Awake() // SingleTon
    {
        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    [Header("TopBar")]
    public GameObject stageScreenBackButton;
    public TextMeshProUGUI actionPointText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI jellyStoneText;

    [Header("MainScreen")]

    [Header("StageScreen")]
    public GameObject stageScreen;  
    public GameObject imageEnemyExplain;  // 적군 설명
    public GameObject imageStageStory;  // 스테이지 번호마다 스토리가 바뀌도록 만들어야 함
    public GameObject settingScreenMain;
    public string selectedStageName = null;

    [Header("HUDScreen")]
    public GameObject HUDsettingScreen;
    public GameObject stageFailScreen;
    public GameObject stageClearScreen;
    public GameObject epicSlimeSkillIcons;
    public GameObject[] epicSlimeSkillTextures;
    public GameObject[] epicSlimeSkillCostText;
    public GameObject shiningIcon;
    public GameObject HUDBackGroundLight;
    public GameObject lockImage;

    public GameObject[] iconBlackImages;
    public GameObject[] slimeSpawnIcons;
    private List<int> originalSiblingIndexes = new List<int>();


    public Texture2D AngelSlimeSkillIcon;
    public Texture2D DevilSlimeSkillIcon;
    public Texture2D WitchSlimeSkillIcon;
    public Texture2D SkullSlimeSkillIcon;

    public Texture2D GhostSlimeSkillIcon;
    public Texture2D LizardSlimeSkillIcon;
    public Texture2D WizardSlimeSkillIcon;
    public Texture2D GrassSlimeSkillIcon;
    public Texture2D CatSlimeSkillIcon;

    public GameObject objectShining;
    public GameObject objectDesImage;
    public TextMeshProUGUI objectDesText;
    public GameObject objectImage;

    [Header("PickUpScreen")]
    public GameObject pickUpScreen;  //슬라임 선택창
    public GameObject notFullSlimeInfo;
    public GameObject onlyOneLegendSlimeInfo;

    [Header("Shop")]
    public GameObject shopScreen;
    public GameObject shopScreenBackButton;
    public GameObject LimitedSalePanel;
    public GameObject AdsShopPanel;
    public GameObject CashShopPanel;

    public TextMeshProUGUI goldAdText; // 광고 개수
    public TextMeshProUGUI jellyStoneAdText; // 광고 개수 

    [Header("Collection")]
    public GameObject collectionScreen;
    public GameObject preButtonCollectionScreen;

    [Header("Etc")]
    public GameObject UIBackGround; // black transparency 100
    public GameObject UIBackGroundLight; 

    [Header("DontDestroy")]
    public GameObject slimeSpawnManager;
    public GameObject mainUI; //메인씬용 스크린
    public GameObject battleHUDScreen;  //HUD 스크린
    public GameObject uIManager;  //UI매니저. 씬 전환시 missing 방지용
    public GameObject slimeManager;  //UI매니저. 씬 전환시 missing 방지용
    public GameObject playerSkillManager;
    public GameObject stageManager;
    public GameObject enhancedObjectManager;
    public GameObject enemySpawnManager;
    public GameObject collectionManager;
    public GameObject aliveSlime;
    public GameObject aliveEnemy;
    public GameObject currenyManager;
    public GameObject dataManager;
    public GameObject lobbySlimeManager;




    #region TapBar

    public void AsycCurrenyUI()
    {
        actionPointText.text = CurrenyManager.Instance.actionPoint.ToString("N0");
        goldText.text = CurrenyManager.Instance.gold.ToString("N0");
        jellyStoneText.text = CurrenyManager.Instance.jellyStone.ToString("N0");
    }

    #endregion

    

    //메인 스크린
    #region MainScreen 
    public void OnClickBattleButton() //배틀 버튼 누르면
    {
        UIBackGround.SetActive(true);
        stageScreen.SetActive(true); //스테이지 화면 열기
        stageScreenBackButton.SetActive(true);
    }

    #endregion

    #region SettingScreenMain

    public void OffSettingScreenMain()
    {
        settingScreenMain.SetActive(false);
        UIBackGroundLight.SetActive(false);
    }
    public void OnSettingScreenMain()
    {
        settingScreenMain.SetActive(true);
        UIBackGroundLight.SetActive(true);
    }
    public void OnClick_OpenURL() // 이용 약관
    {
        Application.OpenURL("https://sites.google.com/view/jellybangeodae"); //연결원하는 사이트
    }
    public void OnClickOpenPersonalURL() // 개인 정보 취급 방침
    {
        Application.OpenURL("https://sites.google.com/view/jellybangeodae"); //연결원하는 사이트
    }

    #endregion

    //스테이지 스크린
    #region StageScreen 
    public void OffStageScreen() //스테이지 나가기 버튼 누르면
    {
        stageScreen.SetActive(false); //스테이지 화면 닫기
        UIBackGround.SetActive(false);
    }

   
    public void OnClickButtonEnemy() //적군을 터치하면
    {
        imageEnemyExplain.SetActive(true); //해당 적군의 설명이 보임
    }

    public void OnClickImageEnemyButton() //적군설명을 터치하면
    {
        imageEnemyExplain.SetActive(false); //해당 적군의 설명이 사라짐
    }
    public void OnClickStageScreenStartButton() //스테이지 시작하기 버튼 누르면
    {
        pickUpScreen.SetActive(true); //픽업화면 열기
        OffImageStageStory();
    }

    public void OnClickStageButton(UnityEngine.UI.Button button) //스테이지를 터치하면 
    {
        selectedStageName = button.name;
        imageStageStory.SetActive(true); //해당 스테이지의 스토리가 보임
    }
    public void OffImageStageStory()
    {
        imageStageStory.SetActive(false);
    }

    #endregion

    //픽업 스크린
    #region PickUpScreen



    public void OffPickUpScreen() 
    {
        pickUpScreen.SetActive(false); //픽업화면 닫기
        InitSlimePickUp();
    }

    private void InitSlimePickUp()
    {
        SlimeManager.instance.InitSlimeSlot();
        SlimeManager.instance.InitSlimeIconCheckImageSetActiveFalse();
    }

    public void OnClickStartButtonPickUpScreen()
    {
        if(SlimeManager.instance.FindFirstEmptySlot() == -1)
        {
            ChangeCanvasToHUD();
            SlimeManager.instance.SelectedSlimes();
            SlimeManager.instance.InitHUDSlimeButton();
            GameManager.Instance.ChangeScene(selectedStageName);
            GameManager.Instance.ResumeGame();
            notFullSlimeInfo.SetActive(false);
        }
        else
        {
            notFullSlimeInfo.SetActive(true);
        }
    }

    public void ChangeCanvasToHUD() //스타트 버튼을 누르면. 픽업씬 스타트 , HUD 설정>리스타트에서도 사용
    {
        HUDsettingScreen.SetActive(false);
        slimeSpawnManager.SetActive(true);//스폰 매니저 켜주기 (젤리력을 위함)
        battleHUDScreen.gameObject.SetActive(true); //HUD화면 캔버스 켜주기
        mainUI.gameObject.SetActive(false); //메인화면 캔버스 켜주기
    }
    #endregion

    #region HUDScreen

    public void OnOffObjectDesText()
    {
        if (!objectShining.activeSelf) return;

        if (objectDesImage.activeSelf)
        {
            objectDesImage.SetActive(false);
        }
        else
        {
            objectDesImage.SetActive(true);
        }
    }

    public void ShuffleSlimeIcon()
    {
        // Save original sibling indexes
        originalSiblingIndexes.Clear();
        foreach (var icon in slimeSpawnIcons)
        {
            originalSiblingIndexes.Add(icon.transform.GetSiblingIndex());
        }

        // Shuffle the slime spawn icons array
        for (int i = 0; i < slimeSpawnIcons.Length; i++)
        {
            GameObject temp = slimeSpawnIcons[i];
            int randomIndex = Random.Range(i, slimeSpawnIcons.Length);
            slimeSpawnIcons[i] = slimeSpawnIcons[randomIndex];
            slimeSpawnIcons[randomIndex] = temp;
        }

        // Apply the shuffled order by setting the sibling index based on the new order
        for (int i = 0; i < slimeSpawnIcons.Length; i++)
        {
            slimeSpawnIcons[i].transform.SetSiblingIndex(i);
        }
    }

    public void ResetOrder()
    {
        // Reset to the original order based on saved sibling indexes
        for (int i = 0; i < originalSiblingIndexes.Count; i++)
        {
            slimeSpawnIcons[i].transform.SetSiblingIndex(originalSiblingIndexes[i]);
        }
    }

    public void UnvisibleSlimeSpawnIcon()
    {
        foreach(GameObject icon in iconBlackImages)
        {
            icon.SetActive(true);
        }

        Invoke("VisibleSlimeSpawnIcon", 7f);
    }
    public void VisibleSlimeSpawnIcon()
    {
        foreach (GameObject icon in iconBlackImages)
        {
            icon.SetActive(false);
        }
    }


    public void OnClickSpeedButton()
    {
        if (!shiningIcon.activeSelf)
        {
            shiningIcon.SetActive(true);
            GameManager.Instance.DoubleTimeScale();
        }
        else
        {
            shiningIcon.SetActive(false);
            GameManager.Instance.OriginTimeScale();
        }

    }

    public void OnOffEpicSlimeSkill()
    {
        epicSlimeSkillIcons.SetActive(!epicSlimeSkillIcons.activeSelf);
    }

    
    public void OnStageClearScreen()
    {
        stageClearScreen.SetActive(true);
    }
    public void OnClickHomeButtonStageClearScreen()
    {
        stageClearScreen.SetActive(false);
        GameManager.Instance.ChangeSceneToMain();
        GameManager.Instance.InitAllStageEnd();
        OnDestroyObjects();
    }
    public void OnStageFailScreen()
    {
        stageFailScreen.SetActive(true);
    }
    public void OnClickHomeButtonStageFailScreen()
    {
        //stageFailScreen.SetActive(false);
        GameManager.Instance.ChangeSceneToMain();
        GameManager.Instance.InitAllStageEnd();
        
        OnDestroyObjects();
    }

    #endregion

    //SettingScreen 안의 버튼 이벤트들 
    #region SettingScreenHUD 

    public void HUDSettingButton() //설정창 들어가기 버튼
    {
        GameManager.Instance.PauseGame();
        HUDsettingScreen.SetActive(true);
        HUDBackGroundLight.SetActive(true);
    }
    public void HUDResumeButton()  //이어서 하기 버튼
    {
        HUDsettingScreen.SetActive(false);
        GameManager.Instance.ResumeGame();
        HUDBackGroundLight.SetActive(false);
    }

    public void OnClickRetryButton() //
    {
        HUDsettingScreen.SetActive(false);

        //Init
        GameManager.Instance.ChangeSceneToMain();
        GameManager.Instance.InitAllStageEnd();
        OnDestroyObjects();

        GameManager.Instance.goPickUpStage = true;
    }

    public void OnClickHelpButton()
    {
        Debug.Log("OnClickHelpButton");
    }
    public void OnClickHomeButton()
    {
        HUDsettingScreen.SetActive(false);
        //Init
        GameManager.Instance.ChangeSceneToMain();
        GameManager.Instance.InitAllStageEnd();
        OnDestroyObjects();
    }

    #endregion

    #region ShopScreen

    public void JellyRefreshButton()
    {
        if (CurrenyManager.Instance.jellyStone - 10 >= 0)
        {
            ShopManager.Instance.purchasePanel.SetActive(false);
            ShopManager.Instance.purchaseSuccessPanel.SetActive(false);
            ShopManager.Instance.purchaseFailPanel.SetActive(false);

            SlimeManager.instance.RefreshShopSlimes();
            CurrenyManager.Instance.jellyStone -= 10;
            AsycCurrenyUI();
            DataManager.Instance.JsonSave();
        }
        else
        {
            // ShopManager.Instance.purchaseFailPanel.SetActive(true);
        }

    }
    public void GoldRefreshButton()
    {
        if (CurrenyManager.Instance.gold - 10 >= 0 && DayManager.Instance.currentGoldRefresh > 0)
        {
            DayManager.Instance.currentGoldRefresh--;

            ShopManager.Instance.purchasePanel.SetActive(false);
            ShopManager.Instance.purchaseSuccessPanel.SetActive(false);
            ShopManager.Instance.purchaseFailPanel.SetActive(false);

            SlimeManager.instance.RefreshShopSlimes();
            CurrenyManager.Instance.gold -= 3000;
            AsycCurrenyUI();
            DayManager.Instance.AsyncGoldRefreshText();
            DataManager.Instance.JsonSave();
        }
        else
        {
            // ShopManager.Instance.purchaseFailPanel.SetActive(true);
        }

    }

    public void OnShopScreen()
    {
        UIBackGround.SetActive(true);
        shopScreen.SetActive(true);
        shopScreenBackButton.SetActive(true);
        OnClickLimitSaleButton(); // Init Shop Store
    }
    public void OffShopScreen()
    {
        UIBackGround.SetActive(false);
        shopScreen.SetActive(false);
        shopScreenBackButton.SetActive(false);
    }

    public void OnClickLimitSaleButton()
    {
        LimitedSalePanel.SetActive(true);
        AdsShopPanel.SetActive(false);
        CashShopPanel.SetActive(false);
    }
    public void OnClickAddShopButton()
    {
        LimitedSalePanel.SetActive(false);
        AdsShopPanel.SetActive(true);
        CashShopPanel.SetActive(false);
    }
    public void OnClickCashStoreButton()
    {
        LimitedSalePanel.SetActive(false);
        AdsShopPanel.SetActive(false);
        CashShopPanel.SetActive(true);
    }

    #endregion

    // shop 안의 버튼 이벤트들 
    #region shop purchase
    public void OnClickFreeGoldButton()
    {
        // 광고 리미트 확인 -> 0이 아니면 
        // 광고 띄우기
        // 재화 추가하기
        // json 저장
        // 광고 리미트 --
        Debug.Log("광고 버튼 눌림 ");
        AdManager.instance.ShowAds(0); // 임시로 골드는 0, 젤리는 1 

    }
    public void OnClickFreeJellyButton()
    {
        Debug.Log("광고 버튼 눌림 ");
        AdManager.instance.ShowAds(1); // 임시로 골드는 0, 젤리는 1 

    }

    // 광고 갯수 표시.. 
    public void InitAdUI(int goldAd, int jellyStoneAd)
    {
        goldAdText.text = goldAd.ToString() + " / 5";
        jellyStoneAdText.text = jellyStoneAd.ToString() + " / 5";
    }

    #endregion

    #region CollectionScreen

    public void OnCollectionScreen()
    {
        UIBackGround.SetActive(true);
        preButtonCollectionScreen.SetActive(true);
        collectionScreen.SetActive(true);
    }

    public void OffCollectionScreen()
    {
        UIBackGround.SetActive(false);
        preButtonCollectionScreen.SetActive(false);
        collectionScreen.SetActive(false);
    }
    

    #endregion

    void OnDestroyObjects()
    {
        //스스로를 파괴하지 않으면 UI연결이 끊기는 문제 발생
        Destroy(mainUI);//메인씬 중복방지용 파괴
        Destroy(slimeSpawnManager);//메인씬 중복방지용 파괴
        Destroy(battleHUDScreen);//메인씬 중복방지용 파괴
        Destroy(uIManager); //전체연결 없어짐 방지
        Destroy(slimeManager);//슬라임 컨텐트 ~ 버튼 연결 없어짐 방지
        Destroy(playerSkillManager);
        Destroy(stageManager);
        Destroy(enhancedObjectManager);
        Destroy(enemySpawnManager);
        Destroy(collectionManager);
        Destroy(aliveSlime);
        Destroy(aliveEnemy);
        Destroy(currenyManager);
        Destroy(dataManager);
        Destroy(lobbySlimeManager);
    }
}
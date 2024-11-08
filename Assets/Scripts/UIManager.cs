﻿using UnityEngine;
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

        slimeSpawnManager.SetActive(false);  
    }
    #endregion

    //사용중인 게임오브젝트들
    #region GameObject

    //메인화면. 행동력, 골드, 젤리력이 쌓이고 재화 값이 보이도록 만들어야 함
    public GameObject shopScreen; //상점스크린
    public GameObject limitedSalePanel; //한정판매
    public GameObject adsshopPanel; //광고상점
    public GameObject wealthStorePanel; //재화상점

    //메인화면>StageScreen
    public GameObject stageScreen;  
    public GameObject imageEnemyExplain;  // 적군 설명
    public GameObject imageStageStory;  // 스테이지 번호마다 스토리가 바뀌도록 만들어야 함
    public GameObject storyClose_Btn;  // 스테이지 스토리를 끄기 위한 패널버튼
    public GameObject buttonChaosClosed;  // 특정 조건을 완료하면 오브젝트 끄기
    public GameObject buttonChaos;  // 카오스 버튼을 누르면 5~8스테이지가 카오스모드로 변해야함
                                    //>2페이지에만 버튼이 있어야할거같음. 나중에 말하기                                
    public GameObject stageScreenChaos;  //카오스 스크린
    public GameObject settingScreen;  //메인화면 세팅

    //HUD > 환경설정
    public GameObject settingScreen2;  //HUD 세팅


    // public GameObject soundScreen;  // 만들어야 함
    public GameObject timeStopPanel;   //시간멈춤확인가능 색 패널

    //SlimePickUpScreen
    public GameObject slimePickUpScreen;  //슬라임 선택창

    [Header("DontDestroy")]
    public GameObject slimeSpawnManager;
    public GameObject mainUI; //메인씬용 스크린
   // public GameObject currencyIndicator; //메인씬 재화표시. 상점에서도 동일한 사용
    public GameObject battleHUDScreen;  //HUD 스크린
    public GameObject uIManager;  //UI매니저. 씬 전환시 missing 방지용
    public GameObject slimeManager;  //UI매니저. 씬 전환시 missing 방지용

    #endregion

    //캔버스 사이즈가 달라질 때 마다 크기 조정. StageScreen 등
    public RectTransform[] stagePages; // 크기를 조정할 UI 요소들


    //메인 스크린
    #region MainScreen 

    public void OnClickShopButton() //ShopScreen Open
    {
        shopScreen.SetActive(true); //
    }
    public void OnClickBattleButton() //배틀 버튼 누르면
    {
        stageScreen.SetActive(true); //스테이지 화면 열기
        GameManager.Instance.ResumeGame();
    }

    #endregion

    //상점 스크린
    #region ShopScreen 
    public void OnClickLimitedSaleButton()
    {
        limitedSalePanel.SetActive(true);
        adsshopPanel.SetActive(false);
        wealthStorePanel.SetActive(false);
    }
    public void OnClickAdsshopButton()
    {
        limitedSalePanel.SetActive(false);
        adsshopPanel.SetActive(true);
        wealthStorePanel.SetActive(false);
    }
    public void OnClickWealthStoreButton()
    {
        limitedSalePanel.SetActive(false);
        adsshopPanel.SetActive(false);
        wealthStorePanel.SetActive(true);
    }



    #endregion

    //스테이지 스크린
    #region StageScreen 
    /*
    public void OnClickStageScreenExitButton() //스테이지 나가기 버튼 누르면
    {
    }*/

    private void ButtonChaosButton() //조건성립시
    {
        //카오스 버튼 해금
    }

    private void RemoveButtonChaosClosed() //특정 스테이지까지 클리어 하면 해금되도록
    {
        buttonChaosClosed.SetActive(false);
    }
    public void OnClickChaosButton() //카오스 버튼 누르면
    {

        stageScreen.SetActive(false); //스테이지 화면 닫기
        stageScreenChaos.SetActive(true);
    }
    public void OnClickChaosModeExitButton() //카오스에서 뒤로가기 누르면 
    {
        stageScreenChaos.SetActive(false);//스테이지 화면 닫기
    }
    public void OnClickNormalButton() //노말모드 버튼 누르면 
    {
        stageScreen.SetActive(true);
        stageScreenChaos.SetActive(false);//스테이지 화면 닫기
    }

    public void OnClickStageButton() //스테이지를 터치하면 
    {
        imageStageStory.SetActive(true); //해당 스테이지의 스토리가 보임
    }
    public void OnClickImageStageStoryButton() //스테이지를 터치하면 
    {
        imageStageStory.SetActive(false); //해당 스테이지의 스토리가 사라짐
        storyClose_Btn.SetActive(false);
        imageEnemyExplain.SetActive(false); //적군설명 닫기
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
        slimePickUpScreen.SetActive(true); //픽업화면 열기
    }


    
    #endregion

    //픽업 스크린
    #region PickUpScreen

    public void OnClickSlimeBackButton() //슬라임픽업창에서 뒤로가기 버튼을 누르면
    {
        slimePickUpScreen.SetActive(false); //픽업화면 닫기
    }
    #endregion
    public void OnClickStartButton() //스타트 버튼을 누르면. 픽업씬 스타트 , HUD 설정>리스타트에서도 사용
    {
        settingScreen2.SetActive(false);
        slimeSpawnManager.SetActive(true);//스폰 매니저 켜주기 (젤리력을 위함)
        battleHUDScreen.gameObject.SetActive(true); //HUD화면 캔버스 켜주기
        mainUI.gameObject.SetActive(false); //메인화면 캔버스 켜주기
        GameManager.Instance.ChangeScene("Stage1"); // 이거 나중에 변수로 ##################### 씬 체인지
        GameManager.Instance.ResumeGame();
    }

    #endregion

    //HUD 안의 버튼 이벤트들 
    #region HUDScreen
    public void OnClickSettingButton2() //설정창 들어가기 버튼
    {
        GameManager.Instance.PauseGame();
        timeStopPanel.SetActive(true);
        settingScreen2.SetActive(true);
    }
    public void OnClilckResumeButton2()  //이어서 하기 버튼
    {
        timeStopPanel.SetActive(false);
        settingScreen2.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
    #endregion

    //SettingScreen 안의 버튼 이벤트들 
    #region SettingScreen
    public void OnClickSettingButton() //설정창 들어가기 버튼
    {
        GameManager.Instance.PauseGame();
        timeStopPanel.SetActive(true);
        settingScreen.SetActive(true);
    }
    public void OnClilckResumeButton()  //이어서 하기 버튼
    {
        timeStopPanel.SetActive(false);
        settingScreen.SetActive(false);
        GameManager.Instance.ResumeGame();
    }


    public void OnClickSoundButton() //만들어야 함
    {
        Debug.Log("OnClickSoundButton");
    }
    public void OnClickBlankButton() //생길지도
    {
        Debug.Log("OnClickBlankButton");
    }

    public void OnClickHelpButton()
    {
        Debug.Log("OnClickHelpButton");
    }
    public void OnClickHomeButton()
    {
        mainUI.SetActive(true); //메인씬 최상위 캔버스 켜주기
     //   SlimeManager.instance.SavePrefabNames();
        OnDestroyObjects();
        SceneManager.LoadScene("MainScreen");
        GameManager.Instance.ResumeGame(); //시간 흐름 되돌리기
        SlimeSpawnManager.instance.jellyPower = 0; //젤리력 초기화

        //SlimeManager.instance.selectedSlimeName.Clear(); //데려갔던 슬라임 리스트 초기화
    }

    #endregion


    //여러 버튼에서 사용되는 경우
    #region Used in multiple buttons
    void OnDestroyObjects()
    {
        //스스로를 파괴하지 않으면 UI연결이 끊기는 문제 발생
        Destroy(mainUI);//메인씬 중복방지용 파괴
        Destroy(slimeSpawnManager);//메인씬 중복방지용 파괴
        Destroy(battleHUDScreen);//메인씬 중복방지용 파괴
        Destroy(uIManager); //전체연결 없어짐 방지
        Destroy(slimeManager);//슬라임 컨텐트 ~ 버튼 연결 없어짐 방지

    }

    //Almost Screens Back_Btn
    public void OnClickBackButton() //뒤로가기 버튼을 누르면
    {
        shopScreen.SetActive(false);//샵화면 닫기
        stageScreen.SetActive(false); //스테이지 화면 닫기
    }

    public void ResizeUI() //폰에 따른 LevelPages 사이즈 조정용
    {
        // mainUI 게임 오브젝트의 RectTransform 컴포넌트의 현재 크기를 가져옵니다.
        RectTransform mainUIRectTransform = mainUI.GetComponent<RectTransform>();
        Vector2 canvasSize = mainUIRectTransform.sizeDelta;

        // stagePages 배열의 각 UI 요소의 크기를 캔버스 크기에 맞춥니다.
        foreach (RectTransform page in stagePages)
        {
            if (page != null)
            {
                page.sizeDelta = new Vector2(canvasSize.x, canvasSize.y);
            }
            else
            {
                Debug.LogError("RectTransform is null in stagePages array");
            }
        }
    }
    #endregion
}
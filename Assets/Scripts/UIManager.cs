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


    //메인화면. 행동력, 골드, 젤리력이 쌓이고 재화 값이 보이도록 만들어야 함

    //메인화면>StageScreen
    public GameObject stageScreen;  
    public GameObject imageEnemyExplain;  // 적군 설명
    public GameObject imageStageStory;  // 스테이지 번호마다 스토리가 바뀌도록 만들어야 함
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
    public GameObject battleHUDScreen;  //HUD 스크린
    public GameObject uIManager;  //UI매니저. 씬 전환시 missing 방지용
    public GameObject slimeManager;  //UI매니저. 씬 전환시 missing 방지용

    //캔버스 사이즈가 달라질 때 마다 크기 조정. StageScreen 등
    //public RectTransform[] stagePages; // 크기를 조정할 UI 요소들

    public string selectedStageName = null;

    //메인 스크린
    #region MainScreen 
    public void OnClickBattleButton() //배틀 버튼 누르면
    {
        stageScreen.SetActive(true); //스테이지 화면 열기
        GameManager.Instance.ResumeGame();
    }

    #endregion

    //스테이지 스크린
    #region StageScreen 
    public void OnClickStageScreenExitButton() //스테이지 나가기 버튼 누르면
    {
        stageScreen.SetActive(false); //스테이지 화면 닫기
    }

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

    
    public void OnClickImageStageStoryButton() //스테이지를 터치하면 
    {
        imageStageStory.SetActive(false); //해당 스테이지의 스토리가 사라짐
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
    /* //TestResizeUI 스크립트를 UIManager에 합치기만 하면 문제가 발생해서 꺼둠
    void ResizeUI() //폰에 따른 LevelPages 사이즈 조정용
    {

        // mainUI 게임 오브젝트의 RectTransform 컴포넌트를 가져옵니다.
        RectTransform canvasRectTransform = mainUI.GetComponent<RectTransform>();
        // 캔버스의 현재 크기를 가져옵니다.
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

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
    */
    #endregion


    //픽업 스크린
    #region PickUpScreen


    public void OnClickStageButton(UnityEngine.UI.Button button) //스테이지를 터치하면 
    {
        selectedStageName = button.name;
        imageStageStory.SetActive(true); //해당 스테이지의 스토리가 보임
    }
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
        GameManager.Instance.ChangeScene(selectedStageName); // 이거 나중에 변수로 ##################### 씬 체인지
        GameManager.Instance.ResumeGame();
    }


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

    void OnDestroyObjects()
    {
        //스스로를 파괴하지 않으면 UI연결이 끊기는 문제 발생
        Destroy(mainUI);//메인씬 중복방지용 파괴
        Destroy(slimeSpawnManager);//메인씬 중복방지용 파괴
        Destroy(battleHUDScreen);//메인씬 중복방지용 파괴
        Destroy(uIManager); //전체연결 없어짐 방지
        Destroy(slimeManager);//슬라임 컨텐트 ~ 버튼 연결 없어짐 방지

    }
}
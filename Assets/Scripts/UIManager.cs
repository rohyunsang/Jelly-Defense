using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;  // Singleton instance


    //DontdisrtoyObject //게임중 Home버튼을 눌러서 메인 씬으로 넘어올때는 파괴해야 함
    // public GameObject dontDestroyObject;
    public GameObject uIManager;
    public GameObject slimeSpawnPoint;
    public GameObject slimeSpawnManager;
    public GameObject canvas_1; //메인씬용 스크린
    public GameObject battleHUDScreen;  //HUD 스크린

    //메인화면. 행동력, 골드, 젤리력이 쌓이고 재화 값이 보이도록 만들어야 함

    //메인화면>StageScreen
    public GameObject stageScreen;  //스테이지 프리팹으로 만들예정. 배열사용할것
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

    //메인 스크린
    #region MainScreen 
    public void OnClickBattleButton() //배틀 버튼 누르면
    {
        stageScreen.SetActive(true); //스테이지 화면 열기
        ResumeGame();
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

    public void OnClickStageButton() //스테이지를 터치하면 
    {
        imageStageStory.SetActive(true); //해당 스테이지의 스토리가 보임
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
        battleHUDScreen.gameObject.SetActive(true); //HUD화면 캔버스 켜주기
        canvas_1.gameObject.SetActive(false); //메인화면 캔버스 켜주기
        SceneManager.LoadScene("StageScreen"); //게임 시작씬으로 이동
        ResumeGame();
    }


    //HUD 안의 버튼 이벤트들 
    #region HUDScreen
    public void OnClickSettingButton2() //설정창 들어가기 버튼
    {
        PauseGame();
        timeStopPanel.SetActive(true);
        settingScreen2.SetActive(true);
    }
    public void OnClilckResumeButton2()  //이어서 하기 버튼
    {
        timeStopPanel.SetActive(false);
        settingScreen2.SetActive(false);
        ResumeGame();
    }
    #endregion

    //SettingScreen 안의 버튼 이벤트들 
    #region SettingScreen
    public void OnClickSettingButton() //설정창 들어가기 버튼
    {
        PauseGame();
        timeStopPanel.SetActive(true);
        settingScreen.SetActive(true);
    }
    public void OnClilckResumeButton()  //이어서 하기 버튼
    {
        timeStopPanel.SetActive(false);
        settingScreen.SetActive(false);
        ResumeGame();
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
        canvas_1.SetActive(true); //메인씬 최상위 캔버스 켜주기
        OnDestroyObjects();
        SceneManager.LoadScene("MainScreen");
        ResumeGame();
    }

    #endregion

    void OnDestroyObjects()
    {
        // Destroy(dontDestroyObject);//메인씬 중복방지용 파괴
        Destroy(uIManager);//메인씬 중복방지용 파괴
        Destroy(canvas_1);//메인씬 중복방지용 파괴
        Destroy(slimeSpawnPoint);//메인씬 중복방지용 파괴
        Destroy(slimeSpawnManager);//메인씬 중복방지용 파괴
        Destroy(battleHUDScreen);//메인씬 중복방지용 파괴
    }


    // 게임 일시정지 함수
    void PauseGame()
    {
        Time.timeScale = 0; // 시간을 멈춤
    }

    // 게임 재개 함수
    void ResumeGame()
    {
        Time.timeScale = 1; // 시간을 다시 시작
    }
}
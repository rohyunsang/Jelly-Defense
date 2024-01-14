using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject settingScreen;
    public GameObject battleHUDScreen;
    private bool isPaused = false; // 게임이 일시정지되었는지 여부를 나타내는 변수

    public void OnClickStartButton()
    {
        startScreen.SetActive(false);
    }
    public void OnClickSettingButton()
    {
        startScreen.SetActive(false);
    }
    void Update()
    {
        // 환경 설정 버튼이나 원하는 키를 눌렀을 때 게임 일시정지 토글
        if (Input.GetKeyDown(KeyCode.Escape)) // 예를 들어, Escape 키로 일시정지 토글
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // 게임 일시정지 함수
    void PauseGame()
    {
        Time.timeScale = 0; // 시간을 멈춤
        isPaused = true;
        // 여기에 일시정지 시 실행할 코드 추가
    }

    // 게임 재개 함수
    void ResumeGame()
    {
        Time.timeScale = 1; // 시간을 다시 시작
        isPaused = false;
        // 여기에 재개 시 실행할 코드 추가
    }
}



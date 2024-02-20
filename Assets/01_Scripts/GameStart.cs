using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStart : MonoBehaviour
{ //TitleScreen > Canvas에 스크립트 넣고 Button_Touch에 할당
    public void OnClickTouchButton() //타이틀화면 터치 버튼 누르면
    {
        SceneManager.LoadScene("Main"); //메인씬으로 이동
    }

}
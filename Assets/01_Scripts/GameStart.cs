using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStart : MonoBehaviour
{ //TitleScreen > Canvas�� ��ũ��Ʈ �ְ� Button_Touch�� �Ҵ�
    public void OnClickTouchButton() //Ÿ��Ʋȭ�� ��ġ ��ư ������
    {
        SceneManager.LoadScene("Main"); //���ξ����� �̵�
    }

}
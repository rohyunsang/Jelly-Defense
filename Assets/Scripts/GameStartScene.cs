using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStartScene : MonoBehaviour
{ //TitleScreen > Canvas�� ��ũ��Ʈ �ְ� Button_Touch�� �Ҵ�
    public void OnClickTouchButton() //Ÿ��Ʋȭ�� ��ġ ��ư ������
    {
        SceneManager.LoadScene("MainScreen"); //���ξ����� �̵�
    }
}

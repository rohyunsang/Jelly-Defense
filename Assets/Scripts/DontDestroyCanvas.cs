using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{ 
    private void Awake()
    {//ImplementMainScreen�� DontDestroy ������Ʈ�� �־
        DontDestroyOnLoad(gameObject); //�ı����� �ʵ��� ��.
    }

}
   


using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{ 
    private void Awake()
    {//ImplementMainScreen�� DontDestroy ������Ʈ�� �־
        DontDestroyOnLoad(gameObject); //�ı����� �ʵ��� ��.
    }

}
   


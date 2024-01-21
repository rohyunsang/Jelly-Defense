using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{ 
    private void Awake()
    {//ImplementMainScreen에 DontDestroy 오브젝트에 넣어서
        DontDestroyOnLoad(gameObject); //파괴하지 않도록 함.
    }

}
   


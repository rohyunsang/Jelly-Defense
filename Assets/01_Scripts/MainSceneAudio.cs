using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneAudio : MonoBehaviour
{
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        AudioManager.Instance.PlayBgm(AudioManager.BGM.BGM_Lobby);
    }

}

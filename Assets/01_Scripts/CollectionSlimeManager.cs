using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionSlimeManager : MonoBehaviour
{

    #region SingleTon Pattern
    public static CollectionSlimeManager Instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        
    }
    #endregion


    public GameObject[] SlimeInfoPanels;
    public int prevIdx = 0;

    public void ShowSlimeInfoPanel(int idx)
    {
        UIManager.instance.UIClickSound();
        SlimeInfoPanels[prevIdx].SetActive(false);
        SlimeInfoPanels[idx].SetActive(true);
        prevIdx = idx;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrenyManager : MonoBehaviour
{
    public static CurrenyManager Instance { get; private set; }
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

    public int actionPoint;
    public int gold;
    public int jellyStone;
}

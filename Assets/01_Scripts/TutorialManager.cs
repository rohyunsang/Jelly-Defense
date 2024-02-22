using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    #region SingleTon Pattern
    public static TutorialManager Instance { get; private set; }
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

    public int currentIdx = 0;

    public GameObject[] tutorialObjs_0;
    public GameObject[] tutorialObjs_1;
    public GameObject[] tutorialObjs_2;
    public GameObject[] tutorialObjs_3;
    public GameObject[] tutorialObjs_4;
    public GameObject[] tutorialObjs_5;
    public GameObject[] tutorialObjs_6;
    public GameObject[] tutorialObjs_7;
    public GameObject[] tutorialObjs_8;
    public GameObject[] tutorialObjs_9;
    public GameObject[] tutorialObjs_10;
    public GameObject[] tutorialObjs_11;
    public GameObject[] tutorialObjs_12;
    public GameObject[] tutorialObjs_13;
    public GameObject[] tutorialObjs_14;
    public GameObject[] tutorialObjs_15;


    public GameObject[][] tutorialObjs;

    public List<string> tutorialStartObjects;
    public List<string> tutorialEndObjects;

    private void Start()
    {
        tutorialObjs = new GameObject[][] {
        tutorialObjs_0, tutorialObjs_1, tutorialObjs_2, tutorialObjs_3,
        tutorialObjs_4, tutorialObjs_5, tutorialObjs_6, tutorialObjs_7,
        tutorialObjs_8, tutorialObjs_9, tutorialObjs_10, tutorialObjs_11,
        tutorialObjs_12, tutorialObjs_13, tutorialObjs_14, tutorialObjs_15
        };

        StartTutorial(0);
    }

    public void StartTutorial(int idx)
    {
        if (idx > 16) return; 

        if (tutorialStartObjects.Contains(idx.ToString()))
        {
            return;
        }

        if (currentIdx >= 0 && currentIdx < tutorialObjs.Length)
        {
            foreach (GameObject obj in tutorialObjs[currentIdx])
            {
                obj.SetActive(true);
            }
        }
        Debug.Log(idx.ToString());
        tutorialStartObjects.Add(idx.ToString());
        if(idx == 8)
        {
            StartCoroutine(DelayedTutorialTransition());
        }

    }

    public void EndTutorial(int idx)
    {
        if (idx > 16) return;

        if (tutorialEndObjects.Contains(idx.ToString()))
        {
            return;
        }
        if (currentIdx >= 0 && currentIdx < tutorialObjs.Length)
        {
            foreach (GameObject obj in tutorialObjs[currentIdx])
            {
                obj.SetActive(false);
            }
        }

        tutorialEndObjects.Add(idx.ToString());
        currentIdx = idx;
        currentIdx++;
    }
    IEnumerator DelayedTutorialTransition()
    {
        

        // 10초 대기
        yield return new WaitForSeconds(10);
        // 현재 튜토리얼 8 종료
        EndTutorial(8);
        // 튜토리얼 9 시작
        StartTutorial(9);
        GameManager.Instance.PauseGame();
    }

}

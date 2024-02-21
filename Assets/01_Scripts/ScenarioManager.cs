using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenarioManager : MonoBehaviour
{

    #region SingleTon Pattern
    public static ScenarioManager Instance { get; private set; }
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

    public GameObject[] introScenarioPages;
    public GameObject[] stagePages_1;
    public GameObject[] stagePages_2;
    public GameObject[] stagePages_3;
    public GameObject[] stagePages_4;
    public GameObject[] stagePages_5;
    public GameObject[] stagePages_6;
    public GameObject[] stagePages_7;
    public GameObject[] stagePages_8;
    public GameObject[] stagePages_9;
    public GameObject[] stagePages_10;

    public GameObject[] stageStoryScreens;



    public int idx = 0;
    public int introScenarioLastPageIdx = 9;

    public int stageScenarioLastPageIdx;
    public int currentStageIndex = 0; // 현재 스테이지 인덱스
    public int currentPageIndex = 0; // 현재 페이지 인덱스

    private GameObject[][] allStageStoryPages; // 모든 스테이지의 페이지를 저장

    private void Start()
    {
        // 스테이지 페이지 초기화
        allStageStoryPages = new GameObject[][] {
            stagePages_1,
            stagePages_2,
            stagePages_3,
            stagePages_4,
            stagePages_5,
            stagePages_6,
            stagePages_7,
            stagePages_8,
            stagePages_9,
            stagePages_10
        };
    }


    public void InitScenarioStageStory(string stageName) //
    {
        GameManager.Instance.PauseGame();
        int n = int.Parse(stageName[stageName.Length - 1].ToString()) - 1;


        // 현재 스테이지 인덱스 업데이트
        currentStageIndex = n;
        // 현재 페이지 인덱스 초기화
        currentPageIndex = 0;

        // 모든 페이지를 비활성화
        foreach (var pageArray in allStageStoryPages)
        {
            foreach (var page in pageArray)
            {
                if (page.name.Contains("Button")) continue;
                
                page.SetActive(false);
            }
        }

        // 현재 스테이지의 첫 번째 페이지 활성화
        if (allStageStoryPages.Length > n && allStageStoryPages[n].Length > 0)
        {
            stageStoryScreens[n].SetActive(true);
            allStageStoryPages[n][0].SetActive(true);
        }

        // 현재 스테이지의 마지막 페이지 인덱스 설정
        stageScenarioLastPageIdx = allStageStoryPages[n].Length - 4; // 배열의 길이에서 1을 빼면 마지막 인덱스가 됨
    }

    // 다음 페이지 버튼 기능
    public void StageScenarioNextButton()
    {
        if (currentPageIndex < allStageStoryPages[currentStageIndex].Length - 1)
        {
            allStageStoryPages[currentStageIndex][currentPageIndex].SetActive(false);
            currentPageIndex++;
            allStageStoryPages[currentStageIndex][currentPageIndex].SetActive(true);
        }
        else
        {
            // 마지막 페이지일 경우의 로직, 예: 다음 스테이지로 넘어가기
        }
    }

    // 이전 페이지 버튼 기능
    public void StageScenarioPrevButton()
    {
        if (currentPageIndex > 0)
        {
            allStageStoryPages[currentStageIndex][currentPageIndex].SetActive(false);
            currentPageIndex--;
            allStageStoryPages[currentStageIndex][currentPageIndex].SetActive(true);
        }
        // 첫 페이지에서 이전 버튼을 누를 경우의 로직, 예: 아무것도 하지 않기
    }

    // 스킵 버튼 기능
    public void StageScenarioSkipButton()
    {
        // 현재 스테이지의 모든 페이지를 비활성화
        foreach (var page in allStageStoryPages[currentStageIndex])
        {
            page.SetActive(false);
        }
        // 스킵 로직, 예: 다음 스테이지로 넘어가기 또는 시나리오 종료 처리
        UIManager.instance.scenarioScreen.SetActive(false); // 가정: 시나리오 스크린을 닫음
    }


    public void introScenarioNextButton()
    {
        introScenarioPages[idx].SetActive(false);
        idx++;
        if (idx > introScenarioLastPageIdx)
        {
            UIManager.instance.scenarioScreen.SetActive(false);
            // intro Cutton End
        }
        else
        {
            introScenarioPages[idx].SetActive(true);
        }
    }
    public void introScenarioPrevButton()
    {
        if (idx <= 0)
        {
            return;
        }

        introScenarioPages[idx].SetActive(false);
        idx--;
        introScenarioPages[idx].SetActive(true);
    }

    public void introScenarioSkipButton()
    {
        UIManager.instance.scenarioScreen.SetActive(false);
    }

}

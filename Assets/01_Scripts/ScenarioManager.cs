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
    public int currentStageIndex = 0; // ���� �������� �ε���
    public int currentPageIndex = 0; // ���� ������ �ε���

    private GameObject[][] allStageStoryPages; // ��� ���������� �������� ����

    private void Start()
    {
        // �������� ������ �ʱ�ȭ
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


        // ���� �������� �ε��� ������Ʈ
        currentStageIndex = n;
        // ���� ������ �ε��� �ʱ�ȭ
        currentPageIndex = 0;

        // ��� �������� ��Ȱ��ȭ
        foreach (var pageArray in allStageStoryPages)
        {
            foreach (var page in pageArray)
            {
                if (page.name.Contains("Button")) continue;
                
                page.SetActive(false);
            }
        }

        // ���� ���������� ù ��° ������ Ȱ��ȭ
        if (allStageStoryPages.Length > n && allStageStoryPages[n].Length > 0)
        {
            stageStoryScreens[n].SetActive(true);
            allStageStoryPages[n][0].SetActive(true);
        }

        // ���� ���������� ������ ������ �ε��� ����
        stageScenarioLastPageIdx = allStageStoryPages[n].Length - 4; // �迭�� ���̿��� 1�� ���� ������ �ε����� ��
    }

    // ���� ������ ��ư ���
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
            // ������ �������� ����� ����, ��: ���� ���������� �Ѿ��
        }
    }

    // ���� ������ ��ư ���
    public void StageScenarioPrevButton()
    {
        if (currentPageIndex > 0)
        {
            allStageStoryPages[currentStageIndex][currentPageIndex].SetActive(false);
            currentPageIndex--;
            allStageStoryPages[currentStageIndex][currentPageIndex].SetActive(true);
        }
        // ù ���������� ���� ��ư�� ���� ����� ����, ��: �ƹ��͵� ���� �ʱ�
    }

    // ��ŵ ��ư ���
    public void StageScenarioSkipButton()
    {
        // ���� ���������� ��� �������� ��Ȱ��ȭ
        foreach (var page in allStageStoryPages[currentStageIndex])
        {
            page.SetActive(false);
        }
        // ��ŵ ����, ��: ���� ���������� �Ѿ�� �Ǵ� �ó����� ���� ó��
        UIManager.instance.scenarioScreen.SetActive(false); // ����: �ó����� ��ũ���� ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public GameObject[] introScenarioPages;

    public int idx = 0;
    public int introScenarioLastPageIdx = 9;

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

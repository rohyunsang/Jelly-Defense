using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using TMPro;

public class SlimeManager : MonoBehaviour
{
    #region SingleTon Pattern
    public static SlimeManager instance { get; private set; }
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set this as the instance and ensure it persists across scenes
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    //[SerializeField] private GoogleSheetManager googleSheetManager;
    public GameObject[] slimePrefabs;
    public GameObject[] slimeIconPrefabs;
    public List<string> selectedSlimeName = new List<string>();
    // show inspector
    public Dictionary<string, bool> hasSlime;

    public GameObject SlimeIconContent;
    public GameObject[] SlimeSlots;
    public GameObject[] SlimeButtons;

//슬라임 슬롯 이름을 저장하고 나중에 다시 연결시키기 위함
    private string[] SlimeSlotNames; 
    private string[] SlimeButtonNames;

    private void Start()
    {
        InitializeDefaultSlimes();
        SpawnSlimeIcon();
        ReconnectPrefabs(); //빈 프리팹 부분 다시 연결해주기
    }

    public void SpawnSlimeIcon()
    {
        foreach (GameObject slimeIconPrefab in slimeIconPrefabs)
        {
            // Instantiate each slime icon prefab as a child of SlimeIconContent
            GameObject slimeIcon = Instantiate(slimeIconPrefab, SlimeIconContent.transform);

            // Additional initialization for the slime icon can be done here
            // For example, setting up UI elements or adding event listeners
        }
    }
    public GameObject GetSlimePrefabByName(string name)
    {
        //반환하라 슬라임 프리팹의 이름이 같은
        return slimePrefabs.FirstOrDefault(slime => slime.name == name);
    }
    public void UpdateSlime()
    {
        // Add Slime ICon Prefab
        // Add Slime Prefab
        // hasSlime["윤상슬라임"] = true;
    }

    private void InitializeDefaultSlimes()
    {
        hasSlime = new Dictionary<string, bool>();
        
        // 기본으로 제공하는 5마리 슬라임
        hasSlime["NormalSlime"] = true;
        hasSlime["PowerSlime"] = true;
        hasSlime["IceSlime"] = true;
        hasSlime["WindSlime"] = true;
        hasSlime["BlockSlime"] = true;

    }

    public void SelectedSlimes() // PickUpScreen Start Button
    {
        foreach (GameObject slot in SlimeSlots)
        {
            // 각 슬롯의 첫 번째 자식 오브젝트를 가져옴
            Transform child = slot.transform.childCount > 0 ? slot.transform.GetChild(0) : null;

            // 자식 오브젝트가 있고, 아직 selectedSlimeName 리스트에 추가되지 않았다면 추가
            if (child != null && !selectedSlimeName.Contains(child.name))
            {
                //selectedSlimeName.Add(child.name);
                selectedSlimeName.Add(child.name.Replace("(Clone)", "")); //프리팹 수정 후 추가되는 이름 Clone 삭제

            }
        }
    }

    public void InitHUDSlimeButton()
    {
        
        for (int i = 0; i < SlimeButtons.Length; i++)
        {/*
            // 선택된 슬라임 이름에 해당하는 아이콘 프리팹 찾기 
            Image iconImage = slimeIconPrefabs.FirstOrDefault(prefab => prefab.name == selectedSlimeName[i]).transform.Find(selectedSlimeName[i]).GetComponent<Image>();
            SlimeButtons[i].transform.Find("Icon").GetComponent<Image>().sprite = iconImage.sprite;*/

            // 선택된 슬라임 이름에 해당하는 아이콘 프리팹 찾기>>프리팹 수정으로 인하여 자식오브젝트의 이미지컴포넌트 이미지를 가져와야 함
            Image iconImage = slimeIconPrefabs.FirstOrDefault(prefab => prefab.name == selectedSlimeName[i]).transform.Find(selectedSlimeName[i]).GetComponent <Image>();

            // SlimeButtons[i]의 자식 오브젝트 "Icon"의 이미지 컴포넌트 가져오기
            Image slimeButtonIconImage = SlimeButtons[i].transform.Find("Icon").GetComponent<Image>();

            // SlimeButtons[i]의 "Icon" 이미지 컴포넌트에 아이콘 이미지 설정
            slimeButtonIconImage.sprite = iconImage.sprite;

            //Cost Search using Linq
            Slime slimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == selectedSlimeName[i]);
            SlimeButtons[i].transform.Find("CostText").GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();

        }
    }

    
    public void SavePrefabNames() //스테이지 - 설정 - 홈버튼을 누르면 실행
    {
        SlimeSlotNames = new string[SlimeSlots.Length];
        SlimeButtonNames = new string[SlimeButtons.Length];

        for (int i = 0; i < SlimeSlots.Length; i++)
        {
            SlimeSlotNames[i] = SlimeSlots[i].name;
        }

        for (int i = 0; i < SlimeButtons.Length; i++)
        {
            SlimeButtonNames[i] = SlimeButtons[i].name;
        }
    }

    void ReconnectPrefabs()// 다시 프리팹을 연결하는 함수
    {
        SlimeSlots = new GameObject[SlimeSlotNames.Length];
        SlimeButtons = new GameObject[SlimeButtonNames.Length];

        for (int i = 0; i < SlimeSlotNames.Length; i++)
        {
            GameObject foundSlot = GameObject.Find(SlimeSlotNames[i]);
            if (foundSlot != null)
            {
                SlimeSlots[i] = foundSlot;
            }
            else
            {
                Debug.LogError("Slot not found: " + SlimeSlotNames[i]);
            }
        }

        for (int i = 0; i < SlimeButtonNames.Length; i++)
        {
            GameObject foundButton = GameObject.Find(SlimeButtonNames[i]);
            if (foundButton != null)
            {
                SlimeButtons[i] = foundButton;
            }
            else
            {
                Debug.LogError("Button not found: " + SlimeButtonNames[i]);
            }
        }
    }

}

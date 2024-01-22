using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using TMPro;
using Button = UnityEngine.UI.Button;

public class SlimeManager : MonoBehaviour
{
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

    public GameObject[] slimePrefabs;
    public GameObject[] slimeIconPrefabs;
    public List<string> selectedSlimeName = new List<string>();
    // show inspector
    public Dictionary<string, bool> hasSlime;

    public GameObject SlimeIconContent;
    public GameObject[] SlimeSlots;
    public GameObject[] SlimeButtons;

    public GameObject slimeInfo;

    private void Start()
    {
        InitializeDefaultSlimes();
        SpawnSlimeIcon();
    }

    #region SlimePickUp

    public void ActivateSlimeInfo()
    {
        slimeInfo.SetActive(true);
    }

    public void SpawnSlimeIcon() // pickUp Screen
    {
        foreach (GameObject slimeIconPrefab in slimeIconPrefabs)
        {
            GameObject slimeIcon = Instantiate(slimeIconPrefab, SlimeIconContent.transform);
            slimeIcon.name = slimeIconPrefab.name;
        }
    }

    // Checks if a specific slot is empty
    public bool IsSlotEmpty(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= SlimeSlots.Length)
        {
            Debug.LogError("Slot index out of range.");
            return false; // Or handle this case differently
        }

        return SlimeSlots[slotIndex].transform.childCount == 0;
    }

    // Find the first empty slot and return its index, or -1 if all slots are full
    public int FindFirstEmptySlot()
    {
        for (int i = 0; i < SlimeSlots.Length; i++)
        {
            if (IsSlotEmpty(i))
            {
                return i;
            }
        }
        return -1; // Indicates all slots are full
    }

    
    #endregion


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
                selectedSlimeName.Add(child.name);
            }
        }
    }

    public void InitHUDSlimeButton()
    {
        
        for (int i = 0; i < SlimeButtons.Length; i++)
        {
            // 선택된 슬라임 이름에 해당하는 아이콘 프리팹 찾기
            Image iconImage = slimeIconPrefabs.FirstOrDefault(prefab => prefab.name == selectedSlimeName[i] + "Icon").transform.Find(selectedSlimeName[i]).GetComponent<Image>();
            SlimeButtons[i].transform.Find("Icon").GetComponent<Image>().sprite = iconImage.sprite;

            //Cost Search using Linq
            Slime slimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == selectedSlimeName[i]);
            SlimeButtons[i].transform.Find("CostText").GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();

        }
    }

    

}

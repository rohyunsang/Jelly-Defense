using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using TMPro;
using Button = UnityEngine.UI.Button;
using Unity.VisualScripting;
using System;

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
    public GameObject[] lobbySlimePrefabs;

    public GameObject[] slimeIconPrefabs;
    public List<string> selectedSlimeName = new List<string>();
    // show inspector
    
    public Dictionary<string, bool> hasSlimes;

    public GameObject SlimeIconContent;
    public GameObject[] SlimeSlots;
    public GameObject[] SlimeButtons;

    public GameObject slimeInfo;

    public List<int> epicSlimeSkillIconIdx;
    public List<string> epicSlimeSkillIconName;

    public string selectedLegendSlime = ""; // 선택된 레전드 슬라임 이름
    public int legendSlimeSpawnIconIdx = -1;

    

    private void Start()
    {
        InitializeDefaultSlimes();
        SpawnSlimeIcon();
        LobbySlimeManager.Instance.RandomInstantiateLobbySlime();
    }

    public void SelectRandomSlimesToShowInShop(int count)
    {
        // 상점에 표시할 슬라임 목록을 초기화
        selectedSlimeName.Clear();

        // 소유한 슬라임 목록에서 랜덤으로 선택
        var ownedSlimes = hasSlimes.Where(s => s.Value == true).Select(s => s.Key).ToList();

        // 소유한 슬라임이 선택할 수 있는 개수보다 적은 경우, 모든 소유한 슬라임을 추가
        if (ownedSlimes.Count <= count)
        {
            selectedSlimeName = ownedSlimes;
        }
        else
        {
            // 선택할 개수만큼 랜덤으로 슬라임을 선택하여 추가
            while (selectedSlimeName.Count < count)
            {
                var randomIndex = UnityEngine.Random.Range(0, ownedSlimes.Count);
                var selectedSlime = ownedSlimes[randomIndex];
                if (!selectedSlimeName.Contains(selectedSlime))
                {
                    selectedSlimeName.Add(selectedSlime);
                }
            }
        }

        // 선택된 슬라임을 기반으로 상점 UI 업데이트 로직 추가
        // 예: 상점 UI에 선택된 슬라임 표시 로직 구현
    }


    #region SlimePickUp

    public void ActivateSlimeInfo()
    {
        slimeInfo.SetActive(true);
    }

    public void SpawnSlimeIcon() // pickUp Screen
    {
        foreach (KeyValuePair<string, bool> slime in hasSlimes)
        {
            Debug.Log($"Slime Name: {slime.Key}, Available: {slime.Value}");
        }
        foreach (GameObject slimeIconPrefab in slimeIconPrefabs)
        {
            // Check if the player owns the slime before spawning its icon.
            if (hasSlimes[slimeIconPrefab.name.Replace("Icon","")])
            {
                GameObject slimeIcon = Instantiate(slimeIconPrefab, SlimeIconContent.transform);
                slimeIcon.name = slimeIconPrefab.name;

                // Optionally, perform additional setup on the slimeIcon here, such as adding listeners or setting up UI elements.
            }
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

    public void InitializeDefaultSlimes()
    {
        hasSlimes = new Dictionary<string, bool>();

        foreach (GameObject slimeIconPrefab in slimeIconPrefabs)
        {
            hasSlimes[slimeIconPrefab.name.Replace("Icon", "")] = false;
        }
        
        // Test 용으로 25마리 다 true
        // 기본으로 제공하는 5마리 슬라임
        hasSlimes["GreenSlime"] = true;
        hasSlimes["WindSlime"] = true;
        hasSlimes["PowerSlime"] = true;
        hasSlimes["SquareIceSlime"] = true;
        hasSlimes["AmethystSlime"] = true;
        hasSlimes["BlockSlime"] = true;
        hasSlimes["BearSlime"] = true;
        hasSlimes["ClownSlime"] = true;
        hasSlimes["BoneSlime"] = true;
        hasSlimes["MagicSlime"] = true;
        hasSlimes["ParabolaSlime"] = true;
        hasSlimes["BloodSlime"] = true;
        hasSlimes["AngelSlime"] = true;
        hasSlimes["CowardSlime"] = true;
        hasSlimes["BunnySlime"] = true;
        hasSlimes["DevilSlime"] = true;
        hasSlimes["WitchSlime"] = true;
        hasSlimes["SkullSlime"] = true;
        hasSlimes["BlockIceSlime"] = true;
        hasSlimes["CupidSlime"] = true;
        hasSlimes["GhostSlime"] = true;
        hasSlimes["LizardSlime"] = true;
        hasSlimes["WizardSlime"] = true;
        hasSlimes["GrassSlime"] = true;
        hasSlimes["CatSlime"] = true;

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
                selectedSlimeName.Add(child.name.Replace("Icon",""));
            }
        }
    }

    public void InitHUDSlimeButton()
    {
        int skillIdx = 0;
        
        for (int i = 0; i < SlimeButtons.Length; i++)
        {
            // 선택된 슬라임 이름에 해당하는 아이콘 프리팹 찾기
            GameObject iconImage = slimeIconPrefabs.FirstOrDefault(prefab => prefab.name == selectedSlimeName[i] + "Icon");
            GameObject iconImageInstance =  Instantiate(iconImage, SlimeButtons[i].transform.position, Quaternion.identity, SlimeButtons[i].transform);
            
            // size
            iconImageInstance.transform.SetSiblingIndex(0);
            RectTransform rectTrans = iconImageInstance.GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(140, 140);

            // delete pickup 
            iconImageInstance.GetComponent<PickUpSlime>().enabled = false;

            //Cost Search using Linq
            Slime slimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == selectedSlimeName[i]);
            SlimeButtons[i].transform.Find("CostText").GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();

            int pickedSlimeIndex = Array.FindIndex(SlimeManager.instance.slimeIconPrefabs, item => item.name == iconImage.name);
            if (pickedSlimeIndex >= 20 && pickedSlimeIndex <= 24)
            {
                SlimeButtons[i].transform.SetAsLastSibling();
                legendSlimeSpawnIconIdx = i;
            }


            if (selectedSlimeName[i] == "AngelSlime")
            {
                Debug.Log("AngelSlime");

                Texture2D texture = UIManager.instance.AngelSlimeSkillIcon;
                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                UIManager.instance.epicSlimeSkillTextures[skillIdx].GetComponent<Image>().sprite = sprite;
                UIManager.instance.epicSlimeSkillTextures[skillIdx].SetActive(true);

                UIManager.instance.epicSlimeSkillCostText[skillIdx].SetActive(true);
                UIManager.instance.epicSlimeSkillCostText[skillIdx].GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();
                epicSlimeSkillIconIdx.Add(skillIdx);
                epicSlimeSkillIconName.Add("AngelSlime");
                skillIdx++;
            }
            else if (selectedSlimeName[i] == "DevilSlime")
            {

                Texture2D texture = UIManager.instance.DevilSlimeSkillIcon;
                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                UIManager.instance.epicSlimeSkillTextures[skillIdx].GetComponent<Image>().sprite = sprite;
                UIManager.instance.epicSlimeSkillTextures[skillIdx].SetActive(true);
                UIManager.instance.epicSlimeSkillCostText[skillIdx].SetActive(true);
                UIManager.instance.epicSlimeSkillCostText[skillIdx].GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();
                epicSlimeSkillIconIdx.Add(skillIdx);
                epicSlimeSkillIconName.Add("DevilSlime");
                skillIdx++;
            }
            else if (selectedSlimeName[i] == "WitchSlime")
            {

                Texture2D texture = UIManager.instance.WitchSlimeSkillIcon;
                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                UIManager.instance.epicSlimeSkillTextures[skillIdx].GetComponent<Image>().sprite = sprite;
                UIManager.instance.epicSlimeSkillTextures[skillIdx].SetActive(true);
                UIManager.instance.epicSlimeSkillCostText[skillIdx].SetActive(true);
                UIManager.instance.epicSlimeSkillCostText[skillIdx].GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();
                epicSlimeSkillIconIdx.Add(skillIdx);
                epicSlimeSkillIconName.Add("WitchSlime");
                skillIdx++;
            }
            else if (selectedSlimeName[i] == "SkullSlime")
            {

                Texture2D texture = UIManager.instance.SkullSlimeSkillIcon;
                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                UIManager.instance.epicSlimeSkillTextures[skillIdx].GetComponent<Image>().sprite = sprite;
                UIManager.instance.epicSlimeSkillTextures[skillIdx].SetActive(true);
                UIManager.instance.epicSlimeSkillCostText[skillIdx].SetActive(true);
                UIManager.instance.epicSlimeSkillCostText[skillIdx].GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();
                epicSlimeSkillIconIdx.Add(skillIdx);
                epicSlimeSkillIconName.Add("SkullSlime");
                skillIdx++;
            }
        }
    }

    public void InitSlimeSlot()
    {
        // 모든 슬라임 슬롯에 대해 반복
        foreach (GameObject slot in SlimeSlots)
        {
            // 각 슬롯의 자식 오브젝트들을 순회하면서 삭제
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void InitSlimeIconCheckImageSetActiveFalse()
    {
        // SlimeIconContent의 모든 자식 오브젝트에 대해 반복
        foreach (Transform child in SlimeIconContent.transform)
        {
            // 각 자식 오브젝트에서 PickUpSlime 컴포넌트를 찾음
            PickUpSlime pickUpSlime = child.GetComponent<PickUpSlime>();

            // PickUpSlime 컴포넌트가 있고, checkImage 게임 오브젝트가 설정되어 있다면
            if (pickUpSlime != null && pickUpSlime.checkImage != null)
            {
                // checkImage 게임 오브젝트를 비활성화
                pickUpSlime.checkImage.SetActive(false);
            }
        }
    }

    public void EpicSlimeSkillController(Button button)
    {
        bool isNotEpic = true;
        if (int.Parse(button.name) >= epicSlimeSkillIconName.Count)
        {
            return;
        }
        int buttonIndex = int.Parse(button.name); 

        // epicSlimeSkillIconName 리스트에서 해당 인덱스의 이름을 사용하여 슬라임을 찾습니다.
        string slimeName = epicSlimeSkillIconName[buttonIndex];
        Slime foundSlime = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == slimeName);

        if (SlimeSpawnManager.instance.jellyPower < foundSlime.Cost) return;
        SlimeSpawnManager.instance.jellyPower -= foundSlime.Cost;
        Debug.Log(foundSlime.Cost);

        
        Debug.Log("button Click");
        Debug.Log(button.name);

        // 부모 GameObject인 slimeParent의 Transform 컴포넌트를 통해 자식들에 접근
        Transform slimeParentTransform = SlimeSpawnManager.instance.slimeParent.transform;

        // 모든 자식 GameObject에 대해 순회
        for (int i = 0; i < slimeParentTransform.childCount; i++)
        {
            // 현재 인덱스의 자식 GameObject를 가져옴
            GameObject childSlime = slimeParentTransform.GetChild(i).gameObject;

            Debug.Log(childSlime.name);
            Debug.Log(int.Parse(button.name));
            Debug.Log(epicSlimeSkillIconName[int.Parse(button.name)]);

            if (childSlime.name.Contains(epicSlimeSkillIconName[int.Parse(button.name)]))
            {
                childSlime.GetComponent<ISlime>().IsSkill = true;
                isNotEpic = false;
            }
        }
        if(isNotEpic) SlimeSpawnManager.instance.jellyPower += foundSlime.Cost;
    }
}


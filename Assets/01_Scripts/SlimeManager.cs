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
    public List<string> epicSLimeSkillIconName;

    private void Start()
    {
        InitializeDefaultSlimes();
        SpawnSlimeIcon();
        LobbySlimeManager.Instance.RandomInstantiateLobbySlime();
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
        //��ȯ�϶� ������ �������� �̸��� ����
        return slimePrefabs.FirstOrDefault(slime => slime.name == name);
    }
    public void UpdateSlime()
    {
        // Add Slime ICon Prefab
        // Add Slime Prefab
        // hasSlime["���󽽶���"] = true;
    }

    public void InitializeDefaultSlimes()
    {
        hasSlimes = new Dictionary<string, bool>();

        foreach (GameObject slimeIconPrefab in slimeIconPrefabs)
        {
            hasSlimes[slimeIconPrefab.name.Replace("Icon", "")] = false;
        }
        
        // Test ������ 25���� �� true
        // �⺻���� �����ϴ� 5���� ������
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
            // �� ������ ù ��° �ڽ� ������Ʈ�� ������
            Transform child = slot.transform.childCount > 0 ? slot.transform.GetChild(0) : null;

            // �ڽ� ������Ʈ�� �ְ�, ���� selectedSlimeName ����Ʈ�� �߰����� �ʾҴٸ� �߰�
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
            // ���õ� ������ �̸��� �ش��ϴ� ������ ������ ã��
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

            if(selectedSlimeName[i] == "AngelSlime")
            {
                Debug.Log("AngelSlime");
                UIManager.instance.addImages[skillIdx].SetActive(false);

                Texture2D texture = UIManager.instance.AngelSlimeSkillIcon;
                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                UIManager.instance.epicSlimeSkillTextures[skillIdx].GetComponent<Image>().sprite = sprite;
                UIManager.instance.epicSlimeSkillTextures[skillIdx].SetActive(true);
                epicSlimeSkillIconIdx.Add(skillIdx);
                epicSLimeSkillIconName.Add("AngelSlime");
                skillIdx++;
            }
            else if (selectedSlimeName[i] == "DevilSlime")
            {
                UIManager.instance.addImages[skillIdx].SetActive(false);

                Texture2D texture = UIManager.instance.DevilSlimeSkillIcon;
                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                UIManager.instance.epicSlimeSkillTextures[skillIdx].GetComponent<Image>().sprite = sprite;
                UIManager.instance.epicSlimeSkillTextures[skillIdx].SetActive(true);
                epicSlimeSkillIconIdx.Add(skillIdx);
                epicSLimeSkillIconName.Add("DevilSlime");
                skillIdx++;
            }
            else if (selectedSlimeName[i] == "WitchSlime")
            {
                UIManager.instance.addImages[skillIdx].SetActive(false);

                Texture2D texture = UIManager.instance.WitchSlimeSkillIcon;
                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                UIManager.instance.epicSlimeSkillTextures[skillIdx].GetComponent<Image>().sprite = sprite;
                UIManager.instance.epicSlimeSkillTextures[skillIdx].SetActive(true);
                epicSlimeSkillIconIdx.Add(skillIdx);
                epicSLimeSkillIconName.Add("WitchSlime");
                skillIdx++;
            }
            else if (selectedSlimeName[i] == "SkullSlime")
            {
                UIManager.instance.addImages[skillIdx].SetActive(false);

                Texture2D texture = UIManager.instance.SkullSlimeSkillIcon;
                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                UIManager.instance.epicSlimeSkillTextures[skillIdx].GetComponent<Image>().sprite = sprite;
                UIManager.instance.epicSlimeSkillTextures[skillIdx].SetActive(true);
                epicSlimeSkillIconIdx.Add(skillIdx);
                epicSLimeSkillIconName.Add("SkullSlime");
                skillIdx++;
            }
        }
    }

    public void InitSlimeSlot()
    {
        // ��� ������ ���Կ� ���� �ݺ�
        foreach (GameObject slot in SlimeSlots)
        {
            // �� ������ �ڽ� ������Ʈ���� ��ȸ�ϸ鼭 ����
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void InitSlimeIconCheckImageSetActiveFalse()
    {
        // SlimeIconContent�� ��� �ڽ� ������Ʈ�� ���� �ݺ�
        foreach (Transform child in SlimeIconContent.transform)
        {
            // �� �ڽ� ������Ʈ���� PickUpSlime ������Ʈ�� ã��
            PickUpSlime pickUpSlime = child.GetComponent<PickUpSlime>();

            // PickUpSlime ������Ʈ�� �ְ�, checkImage ���� ������Ʈ�� �����Ǿ� �ִٸ�
            if (pickUpSlime != null && pickUpSlime.checkImage != null)
            {
                // checkImage ���� ������Ʈ�� ��Ȱ��ȭ
                pickUpSlime.checkImage.SetActive(false);
            }
        }
    }

    public void EpicSlimeSkillController(Button button)
    {
        if (int.Parse(button.name) >= epicSLimeSkillIconName.Count)
        {
            return;
        }
        Debug.Log("button Click");
        Debug.Log(button.name);

        // �θ� GameObject�� slimeParent�� Transform ������Ʈ�� ���� �ڽĵ鿡 ����
        Transform slimeParentTransform = SlimeSpawnManager.instance.slimeParent.transform;

        // ��� �ڽ� GameObject�� ���� ��ȸ
        for (int i = 0; i < slimeParentTransform.childCount; i++)
        {
            // ���� �ε����� �ڽ� GameObject�� ������
            GameObject childSlime = slimeParentTransform.GetChild(i).gameObject;

            Debug.Log(childSlime.name);
            Debug.Log(int.Parse(button.name));
            Debug.Log(epicSLimeSkillIconName[int.Parse(button.name)]);

            if (childSlime.name.Contains(epicSLimeSkillIconName[int.Parse(button.name)]))
            {
                childSlime.GetComponent<ISlime>().IsSkill = true;
            }
        }
    }
}


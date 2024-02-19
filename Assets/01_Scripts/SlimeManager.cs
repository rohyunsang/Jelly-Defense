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
    
    public Dictionary<string, bool> hasSlimes = new Dictionary<string, bool>();

    public GameObject SlimeIconContent;
    public GameObject[] SlimeSlots;
    public GameObject[] SlimeButtons;

    public GameObject slimeInfo;
    public TextMeshProUGUI slimeName;
    public TextMeshProUGUI slimeDesText;
    
    public TextMeshProUGUI slimeHPText;
    public TextMeshProUGUI slimeAttackText;
    public TextMeshProUGUI slimeDefenseText;
    public TextMeshProUGUI slimeMoveSpeedText;
    public TextMeshProUGUI slimeRangeText;
    public TextMeshProUGUI slimeAttackSpeedText;


    public List<int> epicSlimeSkillIconIdx;
    public List<string> epicSlimeSkillIconName;

    public string selectedLegendSlime = ""; // ���õ� ������ ������ �̸�
    public int legendSlimeSpawnIconIdx = -1;

    public Transform shopSlimeParent;
    public GameObject[] shopSlimePrefabs;
    public List<string> showShopSlimes = new List<string>();

    private void Start()
    {
    }

    public void LoadShopSlime(SaveData saveData)
    {
        foreach (var slimeName in saveData.showShopSlimes)
        {
            // ������ �̸��� "Icon"�� �ٿ��� �ش� �������� ������ �������� ã���ϴ�.
            string slimeIconName = slimeName + "Icon";
            GameObject slimeIconPrefab = shopSlimePrefabs.FirstOrDefault(prefab => prefab.name.Equals(slimeIconName));

            // �ش��ϴ� ������ ������ �������� ������ �ν��Ͻ�ȭ�ϰ� ������ �߰��մϴ�.
            if (slimeIconPrefab != null)
            {
                Instantiate(slimeIconPrefab, shopSlimeParent);
                // ������ ǥ�õ� ������ ��Ͽ� ������ �̸��� �߰��մϴ�.
            }
            else
            {
                Debug.LogWarning($"Could not find a prefab for slime: {slimeName}");
            }
        }
    }

    public void RefreshShopSlimes()
    {
        showShopSlimes.Clear();
        Debug.Log("RefreshShopSlimes");

        // ������ ��� �ڽ� ������Ʈ ����
        for (int i = shopSlimeParent.childCount - 1; i >= 0; i--)
        {
            Destroy(shopSlimeParent.GetChild(i).gameObject);
        }

        // hasSlimes���� false�� ������ �����Ӹ� ���͸��Ͽ� ������ �̸��� ���ڿ� ����Ʈ�� ��ȯ
        List<string> availableSlimes = hasSlimes
            .Where(s => !s.Value)
            .Select(s => s.Key + "Icon") // ������ �̸����� ��ȯ
            .ToList();

        // Ȯ���� ���� ������ ����
        List<GameObject> selectedSlimePrefabs = new List<GameObject>();

        // ���õ� �������� ���� 4���� �� ������ �Ǵ� ������ �������� ���� ������ �ݺ�
        while (selectedSlimePrefabs.Count < 4 && availableSlimes.Count > 0)
        {
            foreach (var prefab in shopSlimePrefabs)
            {
                if (selectedSlimePrefabs.Count >= 4) break; // �̹� 4������ �����ߴٸ� �ݺ� �ߴ�

                int index = Array.IndexOf(shopSlimePrefabs, prefab);
                float randomValue = UnityEngine.Random.value; // 0�� 1 ������ ���� ���� ����

                // Ȯ���� ���� ������ ����
                bool isSelected = false;
                if (index <= 5) isSelected = randomValue <= 0.55f;
                else if (index >= 6 && index <= 14) isSelected = randomValue <= 0.35f;
                else if (index >= 15 && index <= 19) isSelected = randomValue <= 0.1f;

                if (isSelected && availableSlimes.Contains(prefab.name))
                {
                    selectedSlimePrefabs.Add(prefab);
                    availableSlimes.Remove(prefab.name); // ���õ� �������� availableSlimes���� ����
                }
            }
        }

        // ���õ� ������ �ν��Ͻ�ȭ �� ó��
        foreach (var slimePrefab in selectedSlimePrefabs)
        {
            Instantiate(slimePrefab, shopSlimeParent);
            string actualSlimeName = slimePrefab.name.Replace("Icon", "");
            showShopSlimes.Add(actualSlimeName);
            Debug.Log("RefreshShopSlimes: Spawned " + actualSlimeName);
        }

        DataManager.Instance.JsonSave();
    }

    #region SlimePickUp

    public void ActivateSlimeInfo()
    {
        slimeInfo.SetActive(true);
    }

    public void SpawnSlimeIcon() // pickUp Screen
    {
        // SlimeIconContent�� ��� �ڽ� ������Ʈ ����
        foreach (Transform child in SlimeIconContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject slimeIconPrefab in slimeIconPrefabs)
        {
            // Check if the player owns the slime before spawning its icon.
            if (hasSlimes[slimeIconPrefab.name.Replace("Icon","")])
            {
                GameObject slimeIcon = Instantiate(slimeIconPrefab, SlimeIconContent.transform);
                slimeIcon.name = slimeIconPrefab.name;

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
    public void UpdateSlime(string slimeName)
    {
        hasSlimes[slimeName] = true;
        SpawnSlimeIcon();
    }

    public void InitializeDefaultSlimes()
    {
        hasSlimes = new Dictionary<string, bool>();

        // Test ������ 25���� �� true
        // �⺻���� �����ϴ� 5���� ������
        hasSlimes["GreenSlime"] = true;
        hasSlimes["WindSlime"] = true;
        hasSlimes["PowerSlime"] = true;
        hasSlimes["SquareIceSlime"] = true;
        hasSlimes["AmethystSlime"] = true;
        hasSlimes["BlockSlime"] = false;
        hasSlimes["BearSlime"] = false;
        hasSlimes["ClownSlime"] = false;
        hasSlimes["BoneSlime"] = false;
        hasSlimes["MagicSlime"] = false;
        hasSlimes["ParabolaSlime"] = false;
        hasSlimes["BloodSlime"] = false;
        hasSlimes["AngelSlime"] = false;
        hasSlimes["CowardSlime"] = false;
        hasSlimes["BunnySlime"] = false;
        hasSlimes["DevilSlime"] = false;
        hasSlimes["WitchSlime"] = false;
        hasSlimes["SkullSlime"] = false;
        hasSlimes["BlockIceSlime"] = false;
        hasSlimes["CupidSlime"] = false;
        hasSlimes["GhostSlime"] = false;
        hasSlimes["LizardSlime"] = false;
        hasSlimes["WizardSlime"] = false;
        hasSlimes["GrassSlime"] = false;
        hasSlimes["CatSlime"] = false;

        //////////////////////////////////////////////////////////////////////// Init
        InitStartSlimeManager();

    }
    //////////////////////////////////////////////////////////////////////// Init
    public void InitStartSlimeManager()
    {
        SpawnSlimeIcon();
        LobbySlimeManager.Instance.RandomInstantiateLobbySlime();
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
        bool isNotEpic = true;
        if (int.Parse(button.name) >= epicSlimeSkillIconName.Count)
        {
            return;
        }
        int buttonIndex = int.Parse(button.name); 

        // epicSlimeSkillIconName ����Ʈ���� �ش� �ε����� �̸��� ����Ͽ� �������� ã���ϴ�.
        string slimeName = epicSlimeSkillIconName[buttonIndex];
        Slime foundSlime = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == slimeName);

        if (SlimeSpawnManager.instance.jellyPower < foundSlime.Cost) return;
        SlimeSpawnManager.instance.jellyPower -= foundSlime.Cost;
        Debug.Log(foundSlime.Cost);

        
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


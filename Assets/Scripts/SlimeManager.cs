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

//������ ���� �̸��� �����ϰ� ���߿� �ٽ� �����Ű�� ����
    private string[] SlimeSlotNames; 
    private string[] SlimeButtonNames;

    private void Start()
    {
        InitializeDefaultSlimes();
        SpawnSlimeIcon();
        ReconnectPrefabs(); //�� ������ �κ� �ٽ� �������ֱ�
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
        //��ȯ�϶� ������ �������� �̸��� ����
        return slimePrefabs.FirstOrDefault(slime => slime.name == name);
    }
    public void UpdateSlime()
    {
        // Add Slime ICon Prefab
        // Add Slime Prefab
        // hasSlime["���󽽶���"] = true;
    }

    private void InitializeDefaultSlimes()
    {
        hasSlime = new Dictionary<string, bool>();
        
        // �⺻���� �����ϴ� 5���� ������
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
            // �� ������ ù ��° �ڽ� ������Ʈ�� ������
            Transform child = slot.transform.childCount > 0 ? slot.transform.GetChild(0) : null;

            // �ڽ� ������Ʈ�� �ְ�, ���� selectedSlimeName ����Ʈ�� �߰����� �ʾҴٸ� �߰�
            if (child != null && !selectedSlimeName.Contains(child.name))
            {
                //selectedSlimeName.Add(child.name);
                selectedSlimeName.Add(child.name.Replace("(Clone)", "")); //������ ���� �� �߰��Ǵ� �̸� Clone ����

            }
        }
    }

    public void InitHUDSlimeButton()
    {
        
        for (int i = 0; i < SlimeButtons.Length; i++)
        {/*
            // ���õ� ������ �̸��� �ش��ϴ� ������ ������ ã�� 
            Image iconImage = slimeIconPrefabs.FirstOrDefault(prefab => prefab.name == selectedSlimeName[i]).transform.Find(selectedSlimeName[i]).GetComponent<Image>();
            SlimeButtons[i].transform.Find("Icon").GetComponent<Image>().sprite = iconImage.sprite;*/

            // ���õ� ������ �̸��� �ش��ϴ� ������ ������ ã��>>������ �������� ���Ͽ� �ڽĿ�����Ʈ�� �̹���������Ʈ �̹����� �����;� ��
            Image iconImage = slimeIconPrefabs.FirstOrDefault(prefab => prefab.name == selectedSlimeName[i]).transform.Find(selectedSlimeName[i]).GetComponent <Image>();

            // SlimeButtons[i]�� �ڽ� ������Ʈ "Icon"�� �̹��� ������Ʈ ��������
            Image slimeButtonIconImage = SlimeButtons[i].transform.Find("Icon").GetComponent<Image>();

            // SlimeButtons[i]�� "Icon" �̹��� ������Ʈ�� ������ �̹��� ����
            slimeButtonIconImage.sprite = iconImage.sprite;

            //Cost Search using Linq
            Slime slimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == selectedSlimeName[i]);
            SlimeButtons[i].transform.Find("CostText").GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();

        }
    }

    
    public void SavePrefabNames() //�������� - ���� - Ȩ��ư�� ������ ����
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

    void ReconnectPrefabs()// �ٽ� �������� �����ϴ� �Լ�
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

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

        // Initialize other components or variables if needed
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


    private void Start()
    {
        InitializeDefaultSlimes();
        SpawnSlimeIcon();
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
                selectedSlimeName.Add(child.name);
            }
        }
    }

    public void InitHUDSlimeButton()
    {
        
        for (int i = 0; i < SlimeButtons.Length; i++)
        {
            // ���õ� ������ �̸��� �ش��ϴ� ������ ������ ã��
            Image iconImage = slimeIconPrefabs.FirstOrDefault(prefab => prefab.name == selectedSlimeName[i]).transform.Find(selectedSlimeName[i]).GetComponent<Image>();
            SlimeButtons[i].transform.Find("Icon").GetComponent<Image>().sprite = iconImage.sprite;

            //Cost Search using Linq
            Slime slimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == selectedSlimeName[i]);
            SlimeButtons[i].transform.Find("CostText").GetComponent<TextMeshProUGUI>().text = slimeData.Cost.ToString();

            
        }
    }
}
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering;

public class SlimeSpawnManager : MonoBehaviour
{
    //instance를 static으로 선언해서 다른 오브젝트에서도 접근 가능
    public static SlimeSpawnManager instance;

    public GameObject[] slimePrefab; //아군 유닛 프리팹
    public Transform spawnPoint; //아군 유닛 스폰 장소 

    public int slimeCost = 0; //아군 유닛 스폰 코스트
    public int maxJellyPower = 250; //아군 유닛 스폰 코스트

    public TMP_Text jellyPowerText; // TextMeshPro Text 오브젝트를 할당할 변수
    public float jellyPower = 250f; // 아군 스폰 코스트 총량

    public bool isStart = false;

    public GameObject slimeParent;

    public bool isEnhanced = false;

    public bool lockJelly = false;

    public bool isAliveLegendSlime = false;
    public Transform legendSlimeSpawnButton;

    public Texture2D originLegendSlimeTexture2D;

    void Awake()
    {
        jellyPower = 0;//젤리력 초기화

        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);
        lockJelly = false;
    }


    void Update()
    {
        if (!isStart) return;
        if (lockJelly) return;
        jellyPower += Time.deltaTime * 15f; //시간 증가시 젤리력 증가
        jellyPower = Mathf.Clamp(jellyPower, 0, maxJellyPower); // Ensure skill power does not exceed max

        // 소수점을 버리고 정수로 변환하여 텍스트로 표시
        int jellyPowerInt = Mathf.FloorToInt(jellyPower);

        // UI Text 오브젝트에 jellyPowerInt 값을 표시
        jellyPowerText.text = jellyPowerInt + " / " + maxJellyPower;
    }

    public void OnClickSlimeIcon(Button button)
    {
        SlimeSpawn(SlimeManager.instance.GetSlimePrefabByName(SlimeManager.instance.selectedSlimeName[int.Parse(button.name)]));
    }

    public void SlimeSpawn(GameObject slimePrefab) // Canvas - Spawn Button 
    {
        // 찾고자 하는 Slime의 Name
        string spawnedSlimeName = slimePrefab.name;

        // slimes 리스트에서 Name이 일치하는 Slime 항목을 찾음
        Slime spawnedSlimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == spawnedSlimeName);


        if (spawnedSlimeData != null)
        {
            // 원하는 Slime을 찾았을 때, 해당 Slime의 정보에 접근할 수 있음
            slimeCost = spawnedSlimeData.Cost;

            Debug.Log("슬라임 코스트:" + slimeCost);
        }
        else
        {
            // 원하는 Slime을 찾지 못한 경우 처리할 내용을 여기에 추가하세요.
            Debug.LogError("Desired Slime with name " + spawnedSlimeName + " not found in the Google Sheet data.");
        }

        if (jellyPower >= slimeCost)
        {
            if (spawnedSlimeData.Grade == 4)
            {
                if (isAliveLegendSlime) {
                    // 슬라임이 살아있으면 여기다가 스킬 처리 
                    jellyPower -= slimeCost;

                    foreach (Transform child in slimeParent.transform)
                    {
                        // 자식 오브젝트의 이름에 slimePrefab.name이 포함되어 있는지 확인합니다.
                        if (child.name.Contains(slimePrefab.name))
                        {
                            // 조건에 맞는 오브젝트를 찾았다면 targetSlime에 참조를 저장합니다.
                            child.gameObject.GetComponent<ISlime>().OnSkill();
                            break; // 첫 번째로 찾은 오브젝트에 대해 처리한 후 반복을 중단합니다.
                        }
                    }

                    return;
                };
                isAliveLegendSlime = true;

                // UIManager에서 특정 슬라임 스폰 아이콘에 접근
                GameObject legendSlimeSpawnIcon = UIManager.instance.slimeSpawnIcons[SlimeManager.instance.legendSlimeSpawnIconIdx];

                // SlimeManager에서 선택된 레전드 슬라임 이름을 가져오고, "Icon"을 제거
                string selectedLegendSlimeName = SlimeManager.instance.selectedLegendSlime.Replace("Icon", "Icon(Clone)");

                Transform selectedLegendSlimeChild = null;

                // legendSlimeSpawnIcon의 자식 오브젝트들 중에서 원하는 이름의 자식 오브젝트 찾기
                foreach (Transform child in legendSlimeSpawnIcon.transform)
                {
                    if (child.name == selectedLegendSlimeName)
                    {
                        selectedLegendSlimeChild = child;
                        break;
                    }
                }

                // 원하는 자식 오브젝트의 자식들에 접근
                if (selectedLegendSlimeChild != null)
                {
                    foreach (Transform grandChild in selectedLegendSlimeChild)
                    {
                        // 여기서 grandChild는 selectedLegendSlimeChild의 자식 오브젝트들입니다.
                        // 필요한 작업 수행

                        if (grandChild.name == selectedLegendSlimeName.Replace("Icon(Clone)", ""))
                        {
                            legendSlimeSpawnButton = grandChild;
                            originLegendSlimeTexture2D = grandChild.GetComponent<Image>().sprite.texture;
                            if(grandChild.name == "GhostSlime")
                            {
                                Texture2D texture = UIManager.instance.GhostSlimeSkillIcon;
                                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                                grandChild.GetComponent<Image>().sprite = sprite;
                            }
                            else if(grandChild.name == "LizardSlime")
                            {
                                Texture2D texture = UIManager.instance.LizardSlimeSkillIcon;
                                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                                grandChild.GetComponent<Image>().sprite = sprite;
                            }
                            else if (grandChild.name == "WizardSlime")
                            {
                                Texture2D texture = UIManager.instance.WizardSlimeSkillIcon;
                                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                                grandChild.GetComponent<Image>().sprite = sprite;
                            }
                            else if (grandChild.name == "GrassSlime")
                            {
                                Texture2D texture = UIManager.instance.GrassSlimeSkillIcon;
                                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                                grandChild.GetComponent<Image>().sprite = sprite;
                            }
                            else if (grandChild.name == "CatSlime")
                            {
                                Texture2D texture = UIManager.instance.CatSlimeSkillIcon;
                                Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                                grandChild.GetComponent<Image>().sprite = sprite;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Selected legend slime child not found");
                }

                // 추가적인 부분. 
                // 1. 레전드 슬라임 스폰 아이콘 (스킬 아이콘)texture로 바꾸기.
                //UIManager.instance.slimeSpawnIcons[SlimeManager.instance.legendSlimeSpawnIconIdx]의 자식오브젝트의 자식오브젝트에 접근한다. 자식 오브젝트의 이름은
                //    SlimeManager.instance.selectedLegendSlime.Replace("Icon", "") 이다. 
                // 1-1. 그럴려면 일단 spawn아이콘
                // 2. 레전드 슬라임 스킬 아이콘 누를시 isSkill() Active. 
                // 3. 레전드 슬라임 사망시 isAliveLegendSlime = false로 바꾸기. 
            }

            // 버튼에 해당하는 슬라임 프리팹을 위치와 회전값을 넣어서 스폰하기
            GameObject spawnedSlime = Instantiate(slimePrefab, spawnPoint.position, spawnPoint.rotation);
            jellyPower -= slimeCost;
            if (isEnhanced && EnhanceObject.Instance.objectType != ObjectType.Jelly) {
                EnhanceObject.Instance.EnhancedSlime(spawnedSlime);
            }
            spawnedSlime.transform.parent = slimeParent.transform;

            int jellyPowerInt = Mathf.FloorToInt(jellyPower);

            // UI Text 오브젝트에 jellyPowerInt 값을 표시
            jellyPowerText.text = jellyPowerInt + " / " + maxJellyPower;
        }
        else
        {
            Debug.Log("jellyPower가 부족합니다");
        }
        
    }

    public void FindSlimeSpawn()
    {
        spawnPoint = GameObject.FindWithTag("SlimeCastle").transform;
    }

    public void InitSlimeSpawnManager()
    {
        isStart = true;
        jellyPower = 0;
    }

    public void StopSlimeSpawnManager()
    {
        isStart = false;
    }

    public void DieLegendSlime()
    {
        isAliveLegendSlime = false;

        Texture2D texture = originLegendSlimeTexture2D;
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        legendSlimeSpawnButton.GetComponent<Image>().sprite = sprite;
    }
}
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
    //instance�� static���� �����ؼ� �ٸ� ������Ʈ������ ���� ����
    public static SlimeSpawnManager instance;

    public GameObject[] slimePrefab; //�Ʊ� ���� ������
    public Transform spawnPoint; //�Ʊ� ���� ���� ��� 

    public int slimeCost = 0; //�Ʊ� ���� ���� �ڽ�Ʈ
    public int maxJellyPower = 250; //�Ʊ� ���� ���� �ڽ�Ʈ

    public TMP_Text jellyPowerText; // TextMeshPro Text ������Ʈ�� �Ҵ��� ����
    public float jellyPower = 250f; // �Ʊ� ���� �ڽ�Ʈ �ѷ�

    public bool isStart = false;

    public GameObject slimeParent;

    public bool isEnhanced = false;

    public bool lockJelly = false;

    public bool isAliveLegendSlime = false;
    public Transform legendSlimeSpawnButton;

    public Texture2D originLegendSlimeTexture2D;

    void Awake()
    {
        jellyPower = 0;//������ �ʱ�ȭ

        // �̹� �ν��Ͻ��� �����ϸ鼭 �̰� �ƴϸ� �ı� ��ȯ
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
        jellyPower += Time.deltaTime * 15f; //�ð� ������ ������ ����
        jellyPower = Mathf.Clamp(jellyPower, 0, maxJellyPower); // Ensure skill power does not exceed max

        // �Ҽ����� ������ ������ ��ȯ�Ͽ� �ؽ�Ʈ�� ǥ��
        int jellyPowerInt = Mathf.FloorToInt(jellyPower);

        // UI Text ������Ʈ�� jellyPowerInt ���� ǥ��
        jellyPowerText.text = jellyPowerInt + " / " + maxJellyPower;
    }

    public void OnClickSlimeIcon(Button button)
    {
        SlimeSpawn(SlimeManager.instance.GetSlimePrefabByName(SlimeManager.instance.selectedSlimeName[int.Parse(button.name)]));
    }

    public void SlimeSpawn(GameObject slimePrefab) // Canvas - Spawn Button 
    {
        // ã���� �ϴ� Slime�� Name
        string spawnedSlimeName = slimePrefab.name;

        // slimes ����Ʈ���� Name�� ��ġ�ϴ� Slime �׸��� ã��
        Slime spawnedSlimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == spawnedSlimeName);


        if (spawnedSlimeData != null)
        {
            // ���ϴ� Slime�� ã���� ��, �ش� Slime�� ������ ������ �� ����
            slimeCost = spawnedSlimeData.Cost;

            Debug.Log("������ �ڽ�Ʈ:" + slimeCost);
        }
        else
        {
            // ���ϴ� Slime�� ã�� ���� ��� ó���� ������ ���⿡ �߰��ϼ���.
            Debug.LogError("Desired Slime with name " + spawnedSlimeName + " not found in the Google Sheet data.");
        }

        if (jellyPower >= slimeCost)
        {
            if (spawnedSlimeData.Grade == 4)
            {
                if (isAliveLegendSlime) {
                    // �������� ��������� ����ٰ� ��ų ó�� 
                    jellyPower -= slimeCost;

                    foreach (Transform child in slimeParent.transform)
                    {
                        // �ڽ� ������Ʈ�� �̸��� slimePrefab.name�� ���ԵǾ� �ִ��� Ȯ���մϴ�.
                        if (child.name.Contains(slimePrefab.name))
                        {
                            // ���ǿ� �´� ������Ʈ�� ã�Ҵٸ� targetSlime�� ������ �����մϴ�.
                            child.gameObject.GetComponent<ISlime>().OnSkill();
                            break; // ù ��°�� ã�� ������Ʈ�� ���� ó���� �� �ݺ��� �ߴ��մϴ�.
                        }
                    }

                    return;
                };
                isAliveLegendSlime = true;

                // UIManager���� Ư�� ������ ���� �����ܿ� ����
                GameObject legendSlimeSpawnIcon = UIManager.instance.slimeSpawnIcons[SlimeManager.instance.legendSlimeSpawnIconIdx];

                // SlimeManager���� ���õ� ������ ������ �̸��� ��������, "Icon"�� ����
                string selectedLegendSlimeName = SlimeManager.instance.selectedLegendSlime.Replace("Icon", "Icon(Clone)");

                Transform selectedLegendSlimeChild = null;

                // legendSlimeSpawnIcon�� �ڽ� ������Ʈ�� �߿��� ���ϴ� �̸��� �ڽ� ������Ʈ ã��
                foreach (Transform child in legendSlimeSpawnIcon.transform)
                {
                    if (child.name == selectedLegendSlimeName)
                    {
                        selectedLegendSlimeChild = child;
                        break;
                    }
                }

                // ���ϴ� �ڽ� ������Ʈ�� �ڽĵ鿡 ����
                if (selectedLegendSlimeChild != null)
                {
                    foreach (Transform grandChild in selectedLegendSlimeChild)
                    {
                        // ���⼭ grandChild�� selectedLegendSlimeChild�� �ڽ� ������Ʈ���Դϴ�.
                        // �ʿ��� �۾� ����

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

                // �߰����� �κ�. 
                // 1. ������ ������ ���� ������ (��ų ������)texture�� �ٲٱ�.
                //UIManager.instance.slimeSpawnIcons[SlimeManager.instance.legendSlimeSpawnIconIdx]�� �ڽĿ�����Ʈ�� �ڽĿ�����Ʈ�� �����Ѵ�. �ڽ� ������Ʈ�� �̸���
                //    SlimeManager.instance.selectedLegendSlime.Replace("Icon", "") �̴�. 
                // 1-1. �׷����� �ϴ� spawn������
                // 2. ������ ������ ��ų ������ ������ isSkill() Active. 
                // 3. ������ ������ ����� isAliveLegendSlime = false�� �ٲٱ�. 
            }

            // ��ư�� �ش��ϴ� ������ �������� ��ġ�� ȸ������ �־ �����ϱ�
            GameObject spawnedSlime = Instantiate(slimePrefab, spawnPoint.position, spawnPoint.rotation);
            jellyPower -= slimeCost;
            if (isEnhanced && EnhanceObject.Instance.objectType != ObjectType.Jelly) {
                EnhanceObject.Instance.EnhancedSlime(spawnedSlime);
            }
            spawnedSlime.transform.parent = slimeParent.transform;

            int jellyPowerInt = Mathf.FloorToInt(jellyPower);

            // UI Text ������Ʈ�� jellyPowerInt ���� ǥ��
            jellyPowerText.text = jellyPowerInt + " / " + maxJellyPower;
        }
        else
        {
            Debug.Log("jellyPower�� �����մϴ�");
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
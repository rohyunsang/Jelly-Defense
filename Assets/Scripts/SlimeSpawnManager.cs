using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using TMPro;

public class SlimeSpawnManager : MonoBehaviour
{
    //instance�� static���� �����ؼ� �ٸ� ������Ʈ������ ���� ����
    public static SlimeSpawnManager instance;

    public GameObject[] slimePrefab; //�Ʊ� ���� ������
    public Transform spawnPoint; //�Ʊ� ���� ���� ��� 

    public int slimeCost = 300; //�Ʊ� ���� ���� �ڽ�Ʈ
    public int maxJellyPower = 500; //�Ʊ� ���� ���� �ڽ�Ʈ

    public TMP_Text jellyPowerText; // TextMeshPro Text ������Ʈ�� �Ҵ��� ����
    public float jellyPower = 0; // �Ʊ� ���� �ڽ�Ʈ �ѷ�

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
    }
    void Update()
    {
        jellyPower += Time.deltaTime * 10f; //�ð� ������ ������ ����

        // �Ҽ����� ������ ������ ��ȯ�Ͽ� �ؽ�Ʈ�� ǥ��
        int jellyPowerInt = Mathf.FloorToInt(jellyPower);

        // UI Text ������Ʈ�� jellyPowerInt ���� ǥ��
        jellyPowerText.text = jellyPowerInt + " / " + maxJellyPower;
    }

    public void OnClickSlimeIcon(Button button)
    {

        //������ ����(�����ӸŴ��� ��ũ��Ʈ�� �Լ��� ����(��ư�� ���� ������ �̸� ����Ʈ�� �̸��� ��������)
        SlimeSpawn(SlimeManager.instance.GetSlimePrefabByName(SlimeManager.instance.selectedSlimeName[int.Parse(button.name)]));
    }

    public void SlimeSpawn(GameObject slimePrefab) // Canvas - Spawn Button 
    {
        // ã���� �ϴ� Slime�� Name
        string spawnedSlimeName = slimePrefab.name;

        // slimes ����Ʈ���� Name�� ��ġ�ϴ� Slime �׸��� ã��
        Slime spawnedSlime = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == spawnedSlimeName);

        if (spawnedSlime != null)
        {
            // ���ϴ� Slime�� ã���� ��, �ش� Slime�� ������ ������ �� ����
            slimeCost = spawnedSlime.Cost;

            Debug.Log("������ �ڽ�Ʈ:" + slimeCost);
        }
        else
        {
            // ���ϴ� Slime�� ã�� ���� ��� ó���� ������ ���⿡ �߰��ϼ���.
            Debug.LogError("Desired Slime with name " + spawnedSlimeName + " not found in the Google Sheet data.");
        }

        if (jellyPower >= slimeCost)
        {
                // ��ư�� �ش��ϴ� ������ �������� ��ġ�� ȸ������ �־ �����ϱ�
            GameObject spawedSlime = Instantiate(slimePrefab, spawnPoint.position, spawnPoint.rotation);
            jellyPower -= slimeCost;
            
        }
        else
        {
            Debug.Log("jellyPower�� �����մϴ�");
        }
        
    }

    public void FindSlimeSpawn()
    {
        Debug.Log(2);
        spawnPoint = GameObject.FindWithTag("SlimeCastle").transform;
    }
}
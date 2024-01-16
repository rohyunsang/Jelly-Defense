using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SlimeSpawnManager : MonoBehaviour
{
    //instance�� static���� �����ؼ� �ٸ� ������Ʈ������ ���� ����
    public static SlimeSpawnManager instance;

    public GameObject[] slimePrefab; //�Ʊ� ���� ������
    public Transform spawnPoint; //�Ʊ� ���� ���� ��� 

    public int slimeCost = 1; //�Ʊ� ���� ���� �ڽ�Ʈ
    public float jellyPower = 0; //�Ʊ� ���� �ڽ�Ʈ �ѷ�
    void Awake()
    {

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
        jellyPower += Time.deltaTime; //�ð� ������ ������ ����
    }

    public void OnClickSlimeIcon(Button button)
    {
        Debug.Log(button.name);
        SlimeSpawn(SlimeManager.instance.GetSlimePrefabByName(SlimeManager.instance.selectedSlimeName[int.Parse(button.name)]));
    }
    
    public void SlimeSpawn(GameObject slimePrefab) // Canvas - Spawn Button 
    {
        if (jellyPower >= slimeCost)
        {
            Instantiate(slimePrefab, spawnPoint.position, spawnPoint.rotation);
            jellyPower -= slimeCost;
            Debug.Log(jellyPower);
        }
        Debug.Log("jellyPower�� �����մϴ�");
    }
}
using UnityEngine;

public class SlimeSpawn : MonoBehaviour
{
    //instance�� static���� �����ؼ� �ٸ� ������Ʈ������ ���� ����
    public static SlimeSpawn instance;

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
    
    public void SpawnSlimePrefab0() // Canvas - Spawn Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index0]);
        Debug.Log("�ε���: " + SlimeManager.Instance.index0); // "�ε���: "�� �Բ� index ���� ���
    }
    public void SpawnSlimePrefab1() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index1]);
        Debug.Log("�ε���: " + SlimeManager.Instance.index1); // "�ε���: "�� �Բ� index ���� ���
    }
    public void SpawnSlimePrefab2() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index2]);
        Debug.Log("�ε���: " + SlimeManager.Instance.index2); // "�ε���: "�� �Բ� index ���� ���
    }
    public void SpawnSlimePrefab3() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index3]);
        Debug.Log("�ε���: " + SlimeManager.Instance.index3); // "�ε���: "�� �Բ� index ���� ���
    }
    public void SpawnSlimePrefab4() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index4]);
        Debug.Log("�ε���: " + SlimeManager.Instance.index4); // "�ε���: "�� �Բ� index ���� ���
    }
    
    /*
    public void SpawnSlimePrefab(int index) // Canvas - Spawn Button
    {
        SlimesSpawn(slimePrefab[index]);
        Debug.Log("�ε���: " + index); // "�ε���: "�� �Բ� index ���� ���
    }*/
    public void SlimesSpawn(GameObject slimePrefab) // Canvas - Spawn Button 
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
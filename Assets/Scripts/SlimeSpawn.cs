using UnityEngine;

public class SlimeSpawn : MonoBehaviour
{
    //instance�� static���� �����ؼ� �ٸ� ������Ʈ������ ���� ����
    public static SlimeSpawn instance;

    public GameObject[] slimePrefab; //�Ʊ� ���� ������
    public Transform spawnPoint; //�Ʊ� ���� ���� ��� 

    public int slimeCost = 1; //�Ʊ� ���� ���� �ڽ�Ʈ
    public float jellyPower = 0; //�Ʊ� ���� �ڽ�Ʈ �ѷ�

    void Update()
    {
        jellyPower += Time.deltaTime; //�ð� ������ ������ ����
    }


    public void SpawnSlimePrefab0() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[0]);
    }
    public void SpawnSlimePrefab1() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[1]);
    }


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
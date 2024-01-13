using UnityEngine;

public class SlimeSpawn : MonoBehaviour
{
    public GameObject[] slimePrefab; //�Ʊ� ���� ������
    public Transform spawnPoint; //�Ʊ� ���� ���� ��� 

    public int slimeCost = 1; //�Ʊ� ���� ���� �ڽ�Ʈ
    public float jellyPower = 0; //�Ʊ� ���� �ڽ�Ʈ �ѷ�

    void Update()
    {
        jellyPower += Time.deltaTime; //�ð� ������ ������ ����
    }
    public void SlimesSpawn() // Canvas - Spawn Button 
    {
        if (jellyPower >= slimeCost)
        {
        Instantiate(slimePrefab[0], spawnPoint.position, spawnPoint.rotation);
        jellyPower -= slimeCost;
        Debug.Log(jellyPower);
        }
        Debug.Log("jellyPower�� �����մϴ�");
    }
    public void SlimesSpawn1() // Canvas - Spawn Button 
    {
        if (jellyPower >= slimeCost)
        {
        Instantiate(slimePrefab[1], spawnPoint.position, spawnPoint.rotation);
        jellyPower -= slimeCost;
        Debug.Log(jellyPower);
        }
        Debug.Log("jellyPower�� �����մϴ�");
    }
}

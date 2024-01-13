using UnityEngine;

public class SlimeSpawn : MonoBehaviour
{
    public GameObject[] slimePrefab; //아군 유닛 프리팹
    public Transform spawnPoint; //아군 유닛 스폰 장소 

    public int slimeCost = 1; //아군 유닛 스폰 코스트
    public float jellyPower = 0; //아군 스폰 코스트 총량

    void Update()
    {
        jellyPower += Time.deltaTime; //시간 증가시 젤리력 증가
    }
    public void SlimesSpawn() // Canvas - Spawn Button 
    {
        if (jellyPower >= slimeCost)
        {
        Instantiate(slimePrefab[0], spawnPoint.position, spawnPoint.rotation);
        jellyPower -= slimeCost;
        Debug.Log(jellyPower);
        }
        Debug.Log("jellyPower가 부족합니다");
    }
    public void SlimesSpawn1() // Canvas - Spawn Button 
    {
        if (jellyPower >= slimeCost)
        {
        Instantiate(slimePrefab[1], spawnPoint.position, spawnPoint.rotation);
        jellyPower -= slimeCost;
        Debug.Log(jellyPower);
        }
        Debug.Log("jellyPower가 부족합니다");
    }
}

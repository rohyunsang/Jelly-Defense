using UnityEngine;

public class SlimeSpawn : MonoBehaviour
{
    //instance를 static으로 선언해서 다른 오브젝트에서도 접근 가능
    public static SlimeSpawn instance;

    public GameObject[] slimePrefab; //아군 유닛 프리팹
    public Transform spawnPoint; //아군 유닛 스폰 장소 

    public int slimeCost = 1; //아군 유닛 스폰 코스트
    public float jellyPower = 0; //아군 스폰 코스트 총량

    void Update()
    {
        jellyPower += Time.deltaTime; //시간 증가시 젤리력 증가
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
        Debug.Log("jellyPower가 부족합니다");
    }
}
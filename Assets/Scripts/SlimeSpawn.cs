using UnityEngine;

public class SlimeSpawn : MonoBehaviour
{
    //instance를 static으로 선언해서 다른 오브젝트에서도 접근 가능
    public static SlimeSpawn instance;

    public GameObject[] slimePrefab; //아군 유닛 프리팹
    public Transform spawnPoint; //아군 유닛 스폰 장소 

    public int slimeCost = 1; //아군 유닛 스폰 코스트
    public float jellyPower = 0; //아군 스폰 코스트 총량
    void Awake()
    {

        // 이미 인스턴스가 존재하면서 이게 아니면 파괴 반환
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
        jellyPower += Time.deltaTime; //시간 증가시 젤리력 증가
    }
    
    public void SpawnSlimePrefab0() // Canvas - Spawn Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index0]);
        Debug.Log("인덱스: " + SlimeManager.Instance.index0); // "인덱스: "와 함께 index 값을 출력
    }
    public void SpawnSlimePrefab1() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index1]);
        Debug.Log("인덱스: " + SlimeManager.Instance.index1); // "인덱스: "와 함께 index 값을 출력
    }
    public void SpawnSlimePrefab2() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index2]);
        Debug.Log("인덱스: " + SlimeManager.Instance.index2); // "인덱스: "와 함께 index 값을 출력
    }
    public void SpawnSlimePrefab3() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index3]);
        Debug.Log("인덱스: " + SlimeManager.Instance.index3); // "인덱스: "와 함께 index 값을 출력
    }
    public void SpawnSlimePrefab4() // Canvas - Spwan Button
    {
        SlimesSpawn(slimePrefab[SlimeManager.Instance.index4]);
        Debug.Log("인덱스: " + SlimeManager.Instance.index4); // "인덱스: "와 함께 index 값을 출력
    }
    
    /*
    public void SpawnSlimePrefab(int index) // Canvas - Spawn Button
    {
        SlimesSpawn(slimePrefab[index]);
        Debug.Log("인덱스: " + index); // "인덱스: "와 함께 index 값을 출력
    }*/
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
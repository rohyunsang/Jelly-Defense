using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;
using TMPro;

public class SlimeSpawnManager : MonoBehaviour
{
    //instance를 static으로 선언해서 다른 오브젝트에서도 접근 가능
    public static SlimeSpawnManager instance;

    public GameObject[] slimePrefab; //아군 유닛 프리팹
    public Transform spawnPoint; //아군 유닛 스폰 장소 

    public int slimeCost = 300; //아군 유닛 스폰 코스트
    public int maxJellyPower = 500; //아군 유닛 스폰 코스트

    public TMP_Text jellyPowerText; // TextMeshPro Text 오브젝트를 할당할 변수
    public float jellyPower = 0; // 아군 스폰 코스트 총량

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
    }
    void Update()
    {
        jellyPower += Time.deltaTime * 10f; //시간 증가시 젤리력 증가

        // 소수점을 버리고 정수로 변환하여 텍스트로 표시
        int jellyPowerInt = Mathf.FloorToInt(jellyPower);

        // UI Text 오브젝트에 jellyPowerInt 값을 표시
        jellyPowerText.text = jellyPowerInt + " / " + maxJellyPower;
    }

    public void OnClickSlimeIcon(Button button)
    {

        //슬라임 스폰(슬라임매니저 스크립트의 함수를 통해(버튼에 따른 슬라임 이름 리스트의 이름들 가져오기)
        SlimeSpawn(SlimeManager.instance.GetSlimePrefabByName(SlimeManager.instance.selectedSlimeName[int.Parse(button.name)]));
    }

    public void SlimeSpawn(GameObject slimePrefab) // Canvas - Spawn Button 
    {
        // 찾고자 하는 Slime의 Name
        string spawnedSlimeName = slimePrefab.name;

        // slimes 리스트에서 Name이 일치하는 Slime 항목을 찾음
        Slime spawnedSlime = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == spawnedSlimeName);

        if (spawnedSlime != null)
        {
            // 원하는 Slime을 찾았을 때, 해당 Slime의 정보에 접근할 수 있음
            slimeCost = spawnedSlime.Cost;

            Debug.Log("슬라임 코스트:" + slimeCost);
        }
        else
        {
            // 원하는 Slime을 찾지 못한 경우 처리할 내용을 여기에 추가하세요.
            Debug.LogError("Desired Slime with name " + spawnedSlimeName + " not found in the Google Sheet data.");
        }

        if (jellyPower >= slimeCost)
        {
                // 버튼에 해당하는 슬라임 프리팹을 위치와 회전값을 넣어서 스폰하기
            GameObject spawedSlime = Instantiate(slimePrefab, spawnPoint.position, spawnPoint.rotation);
            jellyPower -= slimeCost;
            
        }
        else
        {
            Debug.Log("jellyPower가 부족합니다");
        }
        
    }

    public void FindSlimeSpawn()
    {
        Debug.Log(2);
        spawnPoint = GameObject.FindWithTag("SlimeCastle").transform;
    }
}
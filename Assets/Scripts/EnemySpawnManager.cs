using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public GameObject[] enemyPrefab; // 적군 유닛 프리팹
    public Transform spawnPoint; // 아군 유닛 스폰 장소

    public float spawnInterval = 5f; // 스폰되는 시간

    private int[] spawnLimits; // 각 몬스터 유형별 소환 제한
    private int[] spawnCounts; // 각 몬스터 유형별 현재 소환 횟수

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

        spawnCounts = new int[enemyPrefab.Length]; // 소환 횟수를 추적하는 배열 초기화
    }


    public void FindEnemyCastle()
    {
        spawnPoint = GameObject.FindWithTag("EnemyCastle").transform;
    }


    public void EnemySpawnTable(string stageName) // 여기로 접근해야 한다. 
    {
        FindEnemyCastle();
        if (stageName == "Stage1") EnemySpawnStage1();
        if (stageName == "Stage2") EnemySpawnStage2();
        if (stageName == "Stage3") EnemySpawnStage3();
    }
    public void EnemySpawnStage1()
    {
        spawnLimits = new int[enemyPrefab.Length];
        spawnLimits[0] = 5;
        spawnLimits[1] = 5;
        ResetSpawnCounts();
        StartCoroutine(SpawnEnemyWithInterval());
    }

    public void EnemySpawnStage2()
    {
        spawnLimits = new int[enemyPrefab.Length];
        spawnLimits[2] = 5;
        spawnLimits[3] = 5;
        ResetSpawnCounts();
        StartCoroutine(SpawnEnemyWithInterval());
    }

    public void EnemySpawnStage3()
    {
        spawnLimits = new int[enemyPrefab.Length];
        spawnLimits[4] = 5;
        spawnLimits[5] = 5;
        ResetSpawnCounts();
        StartCoroutine(SpawnEnemyWithInterval());
    }

    private void ResetSpawnCounts()
    {
        for (int i = 0; i < spawnCounts.Length; i++)
        {
            spawnCounts[i] = 0;
        }
    }

    private IEnumerator SpawnEnemyWithInterval()
    {
        while (true) // 무한 루프, 종료 조건은 내부에서 처리
        {
            int prefabIndex = Random.Range(0, enemyPrefab.Length); // 랜덤 인덱스 선택

            if (spawnCounts[prefabIndex] < spawnLimits[prefabIndex]) // 소환 제한 확인
            {
                Instantiate(enemyPrefab[prefabIndex], spawnPoint.position, Quaternion.identity);
                spawnCounts[prefabIndex]++;
            }

            yield return new WaitForSeconds(spawnInterval); // 다음 소환까지 대기
        }
    }

}

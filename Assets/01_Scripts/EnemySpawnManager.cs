using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public GameObject[] enemyPrefab; // 적군 유닛 프리팹
    public Transform spawnPoint; // 적군 유닛 스폰 장소

    public float spawnInterval = 5f; // 스폰되는 시간 간격
    private float timeSinceLastSpawn; // 마지막 스폰 이후의 시간

    public int[] spawnLimits; // 각 몬스터 유형별 소환 제한
    public int[] spawnCounts; // 각 몬스터 유형별 현재 소환 횟수

    public GameObject enemyParent;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        spawnCounts = new int[enemyPrefab.Length];
    }

    void Start()
    {
        FindEnemyCastle();
        // 초기 시간 설정
        timeSinceLastSpawn = spawnInterval;
    }

    public void FindEnemyCastle()
    {
        spawnPoint = GameObject.FindWithTag("EnemyCastle").transform;
    }

    public void EnemySpawnTable(string stageName)
    {
        if (stageName == "Stage1") EnemySpawnStage1();
        else if (stageName == "Stage2") EnemySpawnStage2();
        else if (stageName == "Stage3") EnemySpawnStage3();
    }

    public void EnemySpawnStage1()
    {
        spawnLimits = new int[enemyPrefab.Length];
        spawnLimits[0] = 2;
        spawnLimits[1] = 2;
        spawnLimits[2] = 2;
        spawnLimits[3] = 2;
        spawnLimits[4] = 2;
        spawnLimits[5] = 2;
        spawnLimits[6] = 2;
        spawnLimits[7] = 2;
        spawnLimits[8] = 2;
        spawnLimits[9] = 2;
        spawnLimits[11] = 2;
        spawnLimits[12] = 2;
        spawnLimits[13] = 2;
        spawnLimits[14] = 2;
        spawnLimits[15] = 2;
        spawnLimits[16] = 2;
        spawnLimits[17] = 2;
        spawnLimits[18] = 2;
        spawnLimits[19] = 2;
        spawnLimits[20] = 2;
        spawnLimits[21] = 2;
        spawnLimits[22] = 2;
        spawnLimits[23] = 2;
        ResetSpawnCounts();
    }

    public void EnemySpawnStage2()
    {
        spawnLimits = new int[enemyPrefab.Length];
        spawnLimits[2] = 5;
        spawnLimits[3] = 5;
        ResetSpawnCounts();
    }

    public void EnemySpawnStage3()
    {
        spawnLimits = new int[enemyPrefab.Length];
        spawnLimits[4] = 5;
        spawnLimits[5] = 5;
        ResetSpawnCounts();
    }

    private void ResetSpawnCounts()
    {
        for (int i = 0; i < spawnCounts.Length; i++)
        {
            spawnCounts[i] = 0;
        }
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            if (spawnCounts[i] < spawnLimits[i])
            {
                GameObject spawnedEnemy =  Instantiate(enemyPrefab[i], spawnPoint.position, Quaternion.identity);
                spawnCounts[i]++;
                spawnedEnemy.transform.parent = enemyParent.transform;

                break;
            }
        }
    }
}
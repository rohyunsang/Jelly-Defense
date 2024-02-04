using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public GameObject[] enemyPrefab; // ���� ���� ������
    public Transform spawnPoint; // ���� ���� ���� ���

    public float spawnInterval = 5f; // �����Ǵ� �ð� ����
    private float timeSinceLastSpawn; // ������ ���� ������ �ð�

    public int[] spawnLimits; // �� ���� ������ ��ȯ ����
    public int[] spawnCounts; // �� ���� ������ ���� ��ȯ Ƚ��

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
        // �ʱ� �ð� ����
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
        spawnLimits[0] = 5;
        spawnLimits[1] = 5;
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
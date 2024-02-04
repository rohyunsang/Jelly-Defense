using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab; // ��ȯ�� ���� ���� ������
    public int count; // �� ������ ���� �� ���� ��ȯ����

    public EnemySpawnInfo(GameObject prefab, int spawnCount)
    {
        enemyPrefab = prefab;
        count = spawnCount;
    }
}

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public Transform spawnPoint; // ���� ���� ���� ���
    public GameObject enemyParent; // ���� ������ ���� �θ� ������Ʈ
    public GameObject[] enemyPrefab; // �� �迭�� �ν����Ϳ��� �����ؾ� �մϴ�.
    public float spawnInterval = 5f; // �����Ǵ� �ð� ����

    public List<EnemySpawnInfo> currentWaveSpawns = new List<EnemySpawnInfo>(); // ���� ���̺꿡�� ��ȯ�� �� ����

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartWave(List<EnemySpawnInfo> waveSpawns)
    {
        currentWaveSpawns = waveSpawns;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        // ��ü ��ȯ ���� ����Ʈ ����
        List<EnemySpawnInfo> spawnList = new List<EnemySpawnInfo>();
        foreach (var spawnInfo in currentWaveSpawns)
        {
            for (int i = 0; i < spawnInfo.count; i++)
            {
                spawnList.Add(new EnemySpawnInfo(spawnInfo.enemyPrefab, 1));
            }
        }

        // ���� ���� ��ȯ
        while (spawnList.Count > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            int randomIndex = Random.Range(0, spawnList.Count);
            Instantiate(spawnList[randomIndex].enemyPrefab, spawnPoint.position, Quaternion.identity, enemyParent.transform);
            spawnList.RemoveAt(randomIndex); // ��ȯ�� ���ʹ� ����Ʈ���� ����
        }
    }

    IEnumerator StartSequentialWaves(List<List<EnemySpawnInfo>> allWaves)
    {
        foreach (var wave in allWaves)
        {
            currentWaveSpawns = wave;
            yield return StartCoroutine(SpawnWave());
            // ���̺� ���̿� ������ �߰� (��: 5��)
            yield return new WaitForSeconds(5f);
        }
    }

    public void EnemySpawnTable(string stageName) // ���Ⱑ ������.
    {
        InitEnemySpawn();

        if (stageName == "Stage1")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
        {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[0], 4),
                new EnemySpawnInfo(enemyPrefab[1], 4),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[2], 3),
                new EnemySpawnInfo(enemyPrefab[3], 5),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[4], 2),
                new EnemySpawnInfo(enemyPrefab[5], 4),
            }
        };

            StartCoroutine(StartSequentialWaves(allWaves));
        }
    }

    public void InitEnemySpawn()
    {
        spawnPoint = GameObject.FindWithTag("EnemyCastle").transform;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab; // 소환할 적군 유닛 프리팹
    public int count; // 이 유형의 적을 몇 마리 소환할지

    public EnemySpawnInfo(GameObject prefab, int spawnCount)
    {
        enemyPrefab = prefab;
        count = spawnCount;
    }
}

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public Transform spawnPoint; // 적군 유닛 스폰 장소
    public GameObject enemyParent; // 적군 유닛을 담을 부모 오브젝트
    public GameObject[] enemyPrefab; // 이 배열은 인스펙터에서 설정해야 합니다.
    public float spawnInterval = 5f; // 스폰되는 시간 간격

    public List<EnemySpawnInfo> currentWaveSpawns = new List<EnemySpawnInfo>(); // 현재 웨이브에서 소환될 적 정보

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
        // 전체 소환 몬스터 리스트 생성
        List<EnemySpawnInfo> spawnList = new List<EnemySpawnInfo>();
        foreach (var spawnInfo in currentWaveSpawns)
        {
            for (int i = 0; i < spawnInfo.count; i++)
            {
                spawnList.Add(new EnemySpawnInfo(spawnInfo.enemyPrefab, 1));
            }
        }

        // 몬스터 랜덤 소환
        while (spawnList.Count > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            int randomIndex = Random.Range(0, spawnList.Count);
            Instantiate(spawnList[randomIndex].enemyPrefab, spawnPoint.position, Quaternion.identity, enemyParent.transform);
            spawnList.RemoveAt(randomIndex); // 소환된 몬스터는 리스트에서 제거
        }
    }

    IEnumerator StartSequentialWaves(List<List<EnemySpawnInfo>> allWaves)
    {
        foreach (var wave in allWaves)
        {
            currentWaveSpawns = wave;
            yield return StartCoroutine(SpawnWave());
            // 웨이브 사이에 딜레이 추가 (예: 5초)
            yield return new WaitForSeconds(5f);
        }
    }

    public void EnemySpawnTable(string stageName) // 여기가 진입점.
    {
        InitEnemySpawn();

        if (stageName == "Stage1")
        {
            // 모든 웨이브를 포함하는 리스트
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
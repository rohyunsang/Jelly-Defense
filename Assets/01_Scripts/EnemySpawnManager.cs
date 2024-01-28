using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;
    public GameObject[] enemyPrefab; // ���� ���� ������
    public Transform spawnPoint; // �Ʊ� ���� ���� ���

    public float spawnInterval = 5f; // �����Ǵ� �ð�

    private int[] spawnLimits; // �� ���� ������ ��ȯ ����
    private int[] spawnCounts; // �� ���� ������ ���� ��ȯ Ƚ��

    void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϸ鼭 �̰� �ƴϸ� �ı� ��ȯ
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object and make sure it persists between scene loads
        instance = this;
        DontDestroyOnLoad(gameObject);

        spawnCounts = new int[enemyPrefab.Length]; // ��ȯ Ƚ���� �����ϴ� �迭 �ʱ�ȭ
    }


    public void FindEnemyCastle()
    {
        spawnPoint = GameObject.FindWithTag("EnemyCastle").transform;
    }


    public void EnemySpawnTable(string stageName) // ����� �����ؾ� �Ѵ�. 
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
        while (true) // ���� ����, ���� ������ ���ο��� ó��
        {
            int prefabIndex = Random.Range(0, enemyPrefab.Length); // ���� �ε��� ����

            if (spawnCounts[prefabIndex] < spawnLimits[prefabIndex]) // ��ȯ ���� Ȯ��
            {
                Instantiate(enemyPrefab[prefabIndex], spawnPoint.position, Quaternion.identity);
                spawnCounts[prefabIndex]++;
            }

            yield return new WaitForSeconds(spawnInterval); // ���� ��ȯ���� ���
        }
    }

}

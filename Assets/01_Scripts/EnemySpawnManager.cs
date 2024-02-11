using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private float spawnInterval = 5f; // �����Ǵ� �ð� ����

    public List<EnemySpawnInfo> currentWaveSpawns = new List<EnemySpawnInfo>(); // ���� ���̺꿡�� ��ȯ�� �� ����

    public GameObject treasureObject;
    public int currentWave = 1;

    public bool isEnhanced = false;

    public TMP_Text waveText;

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
            GameObject spawnedEnemy =  Instantiate(spawnList[randomIndex].enemyPrefab, spawnPoint.position, Quaternion.identity, enemyParent.transform);
            if (isEnhanced & EnhanceObject.Instance.objectType != ObjectType.Jelly)
            {
                EnhanceObject.Instance.EnhancedEnemy(spawnedEnemy);
            }
            spawnList.RemoveAt(randomIndex); // ��ȯ�� ���ʹ� ����Ʈ���� ����
        }
    }

    IEnumerator StartSequentialWaves(List<List<EnemySpawnInfo>> allWaves)
    {
        foreach (var wave in allWaves) //
        {
            // ���� ���̺� ��ȣ�� UI�� ǥ��
            if (waveText != null)
            {
                waveText.text = $"WAVE {currentWave} / 3";
            }
            int totalMonstersInWave = 0;
            foreach (var spawnInfo in wave)
            {
                totalMonstersInWave += spawnInfo.count;
            }

            // ���̺��� �� �ð��� ���̺��� �� ���� ������ ������ spawnInterval ���
            if (totalMonstersInWave > 5) // 0���� ������ ���� ����
            {
                spawnInterval = 40f / totalMonstersInWave;
            }
            else
            {
                // �� ���� ������ 5���� ������ ��� �⺻ spawnInterval ��� �Ǵ� ���� ó��
                spawnInterval = 5f; // ����: �⺻ ���� ����ϰų� ������ ó���� �� �� �ֽ��ϴ�.
            }
            Debug.Log(spawnInterval);

            currentWaveSpawns = wave;
            yield return StartCoroutine(SpawnWave());
            // ���̺� ���̿� ������ �߰� (��: 5��)
            yield return new WaitForSeconds(5f);
            currentWave++;
            if(currentWave == 2)
            {
                Transform slimeCastleTransform = GameObject.FindWithTag("SlimeCastle").transform;
                Transform enemyCastleTransform = GameObject.FindWithTag("EnemyCastle").transform;
                GameObject spawnedTreasure =  Instantiate(treasureObject, 
                    (slimeCastleTransform.position + enemyCastleTransform.position) / 2 + new Vector3(0f,4f,0f), Quaternion.identity);

                spawnedTreasure.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
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
                new EnemySpawnInfo(enemyPrefab[0], 3),
                new EnemySpawnInfo(enemyPrefab[1], 5),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[0], 4),
                new EnemySpawnInfo(enemyPrefab[1], 4),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[0], 4),
                new EnemySpawnInfo(enemyPrefab[1], 4),
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage2")
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
                new EnemySpawnInfo(enemyPrefab[1], 3),
                new EnemySpawnInfo(enemyPrefab[2], 5),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[1], 4),
                new EnemySpawnInfo(enemyPrefab[2], 4),
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage3")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
            {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[0], 3),
                new EnemySpawnInfo(enemyPrefab[1], 2),
                new EnemySpawnInfo(enemyPrefab[2], 3),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[0], 3),
                new EnemySpawnInfo(enemyPrefab[1], 2),
                new EnemySpawnInfo(enemyPrefab[2], 2),
                new EnemySpawnInfo(enemyPrefab[3], 1),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[0], 3),
                new EnemySpawnInfo(enemyPrefab[1], 2),
                new EnemySpawnInfo(enemyPrefab[2], 1),
                new EnemySpawnInfo(enemyPrefab[3], 2),
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage4")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
            {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[0], 4),
                new EnemySpawnInfo(enemyPrefab[1], 2),
                new EnemySpawnInfo(enemyPrefab[2], 1),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[1], 2),
                new EnemySpawnInfo(enemyPrefab[2], 6),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[0], 1),
                new EnemySpawnInfo(enemyPrefab[1], 1),
                new EnemySpawnInfo(enemyPrefab[24], 1), 
               
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage5")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
            {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[3], 5),
                new EnemySpawnInfo(enemyPrefab[4], 3),
                new EnemySpawnInfo(enemyPrefab[2], 2),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[3], 3),
                new EnemySpawnInfo(enemyPrefab[4], 3),
                new EnemySpawnInfo(enemyPrefab[5], 2),
                new EnemySpawnInfo(enemyPrefab[6], 2),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[5], 5),
                new EnemySpawnInfo(enemyPrefab[6], 5),
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage6")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
            {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[3], 4),
                new EnemySpawnInfo(enemyPrefab[4], 1),
                new EnemySpawnInfo(enemyPrefab[9], 4),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[4], 3),
                new EnemySpawnInfo(enemyPrefab[7], 5),
                new EnemySpawnInfo(enemyPrefab[9], 2),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[3], 3),
                new EnemySpawnInfo(enemyPrefab[8], 2),
                new EnemySpawnInfo(enemyPrefab[9], 4),
                new EnemySpawnInfo(enemyPrefab[11], 1),
                
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage7")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
            {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[6], 3),
                new EnemySpawnInfo(enemyPrefab[7], 5),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[7], 4),
                new EnemySpawnInfo(enemyPrefab[8], 1),
                new EnemySpawnInfo(enemyPrefab[13], 2),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[25], 1) 
                
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage8")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
            {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[6], 3),
                new EnemySpawnInfo(enemyPrefab[8], 2),
                new EnemySpawnInfo(enemyPrefab[10], 3),
                new EnemySpawnInfo(enemyPrefab[11], 2),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[4], 2),
                new EnemySpawnInfo(enemyPrefab[6], 3),
                new EnemySpawnInfo(enemyPrefab[9], 3),
                new EnemySpawnInfo(enemyPrefab[16], 2),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[6], 1), 
                new EnemySpawnInfo(enemyPrefab[7], 2),
                new EnemySpawnInfo(enemyPrefab[10], 4),
                new EnemySpawnInfo(enemyPrefab[12], 2),
                new EnemySpawnInfo(enemyPrefab[13], 1),
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage9")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
            {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[7], 3),
                new EnemySpawnInfo(enemyPrefab[8], 3),
                new EnemySpawnInfo(enemyPrefab[9], 3),
                new EnemySpawnInfo(enemyPrefab[12], 1),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[7], 4),
                new EnemySpawnInfo(enemyPrefab[8], 2),
                new EnemySpawnInfo(enemyPrefab[9], 2),
                new EnemySpawnInfo(enemyPrefab[12], 2),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[7], 2),
                new EnemySpawnInfo(enemyPrefab[8], 1),
                new EnemySpawnInfo(enemyPrefab[9], 3),
                new EnemySpawnInfo(enemyPrefab[10], 1),
                new EnemySpawnInfo(enemyPrefab[16], 3),
            }
            };
            StartCoroutine(StartSequentialWaves(allWaves));
        }
        else if (stageName == "Stage10")
        {
            // ��� ���̺긦 �����ϴ� ����Ʈ
            List<List<EnemySpawnInfo>> allWaves = new List<List<EnemySpawnInfo>>
            {
            // Wave1
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[6], 2),
                new EnemySpawnInfo(enemyPrefab[5], 2),
                new EnemySpawnInfo(enemyPrefab[9], 2),
                new EnemySpawnInfo(enemyPrefab[11], 2),
            },
            // Wave2
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[6], 2),
                new EnemySpawnInfo(enemyPrefab[8], 2),
                new EnemySpawnInfo(enemyPrefab[9], 2),
                new EnemySpawnInfo(enemyPrefab[11], 2),
            },
            // Wave3
            new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo(enemyPrefab[26], 1) 

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
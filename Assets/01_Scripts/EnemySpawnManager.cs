using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private float spawnInterval = 5f; // 스폰되는 시간 간격

    public List<EnemySpawnInfo> currentWaveSpawns = new List<EnemySpawnInfo>(); // 현재 웨이브에서 소환될 적 정보

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
            GameObject spawnedEnemy =  Instantiate(spawnList[randomIndex].enemyPrefab, spawnPoint.position, Quaternion.identity, enemyParent.transform);
            if (isEnhanced & EnhanceObject.Instance.objectType != ObjectType.Jelly)
            {
                EnhanceObject.Instance.EnhancedEnemy(spawnedEnemy);
            }
            spawnList.RemoveAt(randomIndex); // 소환된 몬스터는 리스트에서 제거
        }
    }

    IEnumerator StartSequentialWaves(List<List<EnemySpawnInfo>> allWaves)
    {
        foreach (var wave in allWaves) //
        {
            // 현재 웨이브 번호를 UI에 표시
            if (waveText != null)
            {
                waveText.text = $"WAVE {currentWave} / 3";
            }
            int totalMonstersInWave = 0;
            foreach (var spawnInfo in wave)
            {
                totalMonstersInWave += spawnInfo.count;
            }

            // 웨이브의 총 시간을 웨이브의 총 몬스터 개수로 나누어 spawnInterval 계산
            if (totalMonstersInWave > 5) // 0으로 나누는 것을 방지
            {
                spawnInterval = 40f / totalMonstersInWave;
            }
            else
            {
                // 총 몬스터 개수가 5마리 이하인 경우 기본 spawnInterval 사용 또는 오류 처리
                spawnInterval = 5f; // 예시: 기본 값을 사용하거나 적절한 처리를 할 수 있습니다.
            }
            Debug.Log(spawnInterval);

            currentWaveSpawns = wave;
            yield return StartCoroutine(SpawnWave());
            // 웨이브 사이에 딜레이 추가 (예: 5초)
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
            // 모든 웨이브를 포함하는 리스트
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
            // 모든 웨이브를 포함하는 리스트
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
            // 모든 웨이브를 포함하는 리스트
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
            // 모든 웨이브를 포함하는 리스트
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
            // 모든 웨이브를 포함하는 리스트
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
            // 모든 웨이브를 포함하는 리스트
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
            // 모든 웨이브를 포함하는 리스트
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
            // 모든 웨이브를 포함하는 리스트
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
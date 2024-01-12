using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    public GameObject playerPrefab; //아군 유닛 프리팹
    public Transform spawnPoint; //아군 유닛 스폰 장소 
   
    public float jellyPower = 0; //아군 유닛 소환 젤리력 코스트
    private NavMeshAgent slimeAgent; 
    public Transform enemyCastle;
    public float detectionRadius = 1f; //아군 유닛의 적 감지 반경
    Transform closestEnemy;

    public int slimeHP = 10; //아군 유닛 체력
    public float damageCooldown = 1f; // 1초에 한 번씩만 체력이 감소하도록 설정
    private float nextDamageTime;
    bool isDead = false;

    private Animator animator; 

   // private float timeUpdate = 0.0f; //시간 업데이트

    void Awake()
    {
        slimeAgent = playerPrefab.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        slimeAgent.SetDestination(enemyCastle.position);
    }
    void Update()
    {

        if (slimeHP <= 0)
        {
            StopSlimeAgent();
            slimeAgent.enabled = false;
            Die();
        }

        jellyPower += Time.deltaTime; //시간 증가시 젤리력 증가

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Debug.Log("Enemy");
                float enemyDistance = Vector3.Distance(transform.position, col.transform.position);

                if (enemyDistance < closestDistance)
                {
                    closestDistance = enemyDistance;
                       closestEnemy = col.transform;
                }
            }
        }
        if (closestEnemy != null)
        {
            // 가장 가까운 적을 향해 이동
            slimeAgent.SetDestination(closestEnemy.position);
            animator.SetTrigger("Attack01");
        }
        else
        {
            // 가장 가까운 적이 없으면 적군의 성을 향해 이동
            slimeAgent.SetDestination(enemyCastle.position);
            animator.SetBool("isMove",true);
        }
    }

    void StopSlimeAgent()
    {
        // NavMeshAgent를 멈추기
        if (slimeAgent != null)
        {
            slimeAgent.isStopped = true;
        }
    }
    public void PlayerSpawn() // Canvas - Spawn Button 
    {
        slimeHP = 10;
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        jellyPower -= 1f;
        Debug.Log(jellyPower);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && Time.time >= nextDamageTime)
        {
            GetHit(1); // HP를 1 감소
            nextDamageTime = Time.time + damageCooldown; // 다음 데미지 시간 설정
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Time.time >= nextDamageTime)
        {
            GetHit(1); // HP를 1 감소
            nextDamageTime = Time.time + damageCooldown; // 다음 데미지 시간 설정
            if (isDead)
            {
            }
        }
    }
    void GetHit(int damage)
    {
        slimeHP -= damage;
        Debug.Log(slimeHP);
        if (slimeHP <= 0)
        {
            Invoke("Die",5);
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");
        slimeAgent.enabled = true;
        Destroy(gameObject);
    }

}
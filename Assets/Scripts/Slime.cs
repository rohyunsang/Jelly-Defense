using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    public int HP = 10; //아군 유닛 체력
    public float detectionRadius = 10f; //아군 유닛의 적 감지 반경
    public float damageInterval = 1f; // 데미지를 받을 주기
    private float nextDamageTime; //다음 데미지를 받을 타이밍

    private Animator animator;
    private NavMeshAgent navAgent;
    private Transform enemyCastle; //프리팹 사용시 null이 되는 오류 방지를 위해 private로 변경

    bool isDead = false;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        navAgent.enabled = true;
        navAgent.isStopped = false;
        //게임오브젝트 중 적군 성 태그를 가진 오브젝트의 트랜스폼을 향해 가도록 함
        GameObject enemyCastleObject = GameObject.FindGameObjectWithTag("EnemyCastle");
        if (enemyCastleObject != null)
        {
            enemyCastle = enemyCastleObject.transform;
            navAgent.SetDestination(enemyCastle.position);
        }
        else
        {
            Debug.LogError("EnemyCastle not found in the scene.");
        }
    
        // NavMeshAgent를 초기화할 때 NavMesh에 적용되어 있어야 합니다.
        if (navAgent.isOnNavMesh)
        {
            // NavMeshAgent를 활성화하고 초기 위치로 이동
            navAgent.enabled = true;
            navAgent.SetDestination(transform.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent is not on NavMesh!");
        }
    }
    void Update()
    {
        if (isDead) return;


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius); //범위 콜리더 감지
        Transform closestEnemy = FindClosestEnemy(hitColliders); //가까운 적의 위치

        if (closestEnemy != null)
        {
            // 가장 가까운 적을 향해 이동 //#이게 성을 떄리다가도 주변에 몬스터가 생성되면 몬스터를 공격해야함.
            navAgent.SetDestination(closestEnemy.position);
        }
        else
        {
            // Enemy 태그를 가진 오브젝트 감지되지 않으면 EnemyCastle 태그를 가진 오브젝트를 따라감
            GameObject enemyCastleObject = GameObject.FindGameObjectWithTag("EnemyCastle");

            if (enemyCastleObject != null)
            {
                enemyCastle = enemyCastleObject.transform;
                navAgent.SetDestination(enemyCastle.position);
            }

            animator.SetBool("isMove", true);
        }
    }

    Transform FindClosestEnemy(Collider[] colliders)
    {
        Transform closestEnemy = null; //가장 가까운 적의 위치
        float closestDistance = Mathf.Infinity; //가장 가까운적의 거리

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy")) //적 태그면
            {//현재 슬라임과 적의 거리 계산
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = col.transform;
                }
            }
        }
        
        return closestEnemy;
    }

    private void OnTriggerEnter(Collider other) //물리 충돌로 인한 밀려남 방지
    {
        SlimeCollision(other);
    }
    private void OnTriggerStay(Collider other)
    {
        SlimeCollision(other);
    }

    void SlimeCollision(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyCastle") && Time.time >= nextDamageTime)
        {
            animator.SetTrigger("Attack01");
            nextDamageTime = Time.time + damageInterval;
        }
        if ((other.gameObject.CompareTag("Enemy")) && Time.time >= nextDamageTime)
        {
            animator.SetTrigger("Attack01");
            GetHit(1);
            nextDamageTime = Time.time + damageInterval;
        }
    }

    void GetHit(int damage)
    {
        HP -= damage;
        Debug.Log(HP);
        if (HP <= 0)
        {
            isDead = true;
            //# 여기서 막을 건 다막아야됨 
            //# rigidbody끄고 
            StopNavAgent(); //# 어차피 죽은애니까 멈춰도 좋아요 
            navAgent.enabled = false; //# NavMeshAgent끄고
            animator.SetTrigger("Death");
            Invoke("Die", 1);
        }
    }

    void StopNavAgent()
    {
        if (navAgent != null && navAgent.isOnNavMesh && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true;
        }
    }
    void Die() //# 핵심 문제점 Die() 
    {
        Destroy(gameObject);
    }

}
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform slimeCastle; // 슬라임 성 위치
    private NavMeshAgent navAgent;
    private Animator anim;

    //피격(아직 테스팅용)
    public float damageInterval = 1f; // 데미지를 받을 주기
    private float nextDamageTime; //다음 데미지를 받을 타이밍
    

    //이동용
    public float detectionRadius = 10f; //적 감지 반경
    private float detectionInterval = 0.5f;  // 범위 탐지 주기
    private float sinceLastDetectionTime = 0f; // 탐지 주기 초기화용
    private Transform target; // current target 적

    //공격, 공격력(예정)
    public float attackDistance = 3f; // 공격 가능 거리
    public float attackInterval = 1f; //다음 공격 주기
    private float nextAttackTime; //공격주기 누적 초기화용

    public int HP = 100;
    bool isDead = false;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        slimeCastle = GameObject.FindWithTag("SlimeCastle").transform;
    }

    void Start()
    {
        //게임오브젝트 중 적군 성 태그를 가진 오브젝트의 트랜스폼을 향해 가도록 함

        if (slimeCastle != null)
        {
            target = slimeCastle.transform;  //타겟에 넣기
            MoveToTarget(target);// 시작 시 적군 성으로 이동
        }
        else
        {
            Debug.LogError("EnemyCastle not found in the scene.");
        }

        // NavMeshAgent를 초기화할 때 NavMesh에 적용되어 있어야 함. 없으면 에러발생
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

        sinceLastDetectionTime += Time.deltaTime;
        if (sinceLastDetectionTime >= detectionInterval)
        {
            SearchSlimeInDetection();
            sinceLastDetectionTime = 0f;
        }

        if (target != null) //타겟이 있으면
        {
            MoveToTarget(target); //타겟을향해 네비메쉬 이동

            float distanceToTarget = Vector3.Distance(transform.position, target.position); //타겟과의 간격계산
            if (distanceToTarget <= attackDistance) //공격범위 이하의 간격이면
            {
                if (Time.time >= nextAttackTime)//공격 쿨타임에 맞춰서 
                {
                    Attack(); //공격, 애니메이션이 주기적으로 나오게 하기 위함
                    nextAttackTime = Time.time + attackInterval; //공격 쿨타임 누적 초기화용
                }
            }
        }
        else
        {
            float currentVelocity = navAgent.velocity.magnitude;// 움직임 여부를 판단
            if (currentVelocity <= 1f)
            {
                anim.SetBool("isMove", false); //idle 애니메이션 실행
            }
            else
            {
                anim.SetBool("isMove", true); //이동(idle2) 애니메이션 실행
            }
        }
    }

    void SearchSlimeInDetection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius); //범위 콜리더 감지, 배열 저장
        Transform closestSlime = FindClosestSlime(hitColliders); //가까운 슬라임의 위치 저장

        if (closestSlime != null)
        {
            target = closestSlime; //가까운 적의 위치로 이동
        }
        else
        {
            target = slimeCastle.transform; //슬라임 성으로 이동
        }
    }

    Transform FindClosestSlime(Collider[] colliders)
    {
        Transform closestSlime = null; //가장 가까운 슬라임의 위치
        float closestDistance = Mathf.Infinity; //가장 가까운적의 거리

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Slime")) //슬라임 태그면
            {
                //현재 적과 슬라임의 거리 계산
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance) //적 거리가 가장 가까운적과의 간격보다 작으면
                {
                    closestDistance = distanceToEnemy; //적 거리를 가장 가까운 적과의 간격에 넣기
                    closestSlime = col.transform; //콜리더의 위치를 가장 가까운 적의 위치에 넣기
                }
            }
        }

        return closestSlime; //가장 가까운 적의위치를 반환
    }

    void MoveToTarget(Transform target)
    {
        navAgent.SetDestination(target.position); // 지정된 타겟으로 이동
    }

    void Attack()
    {
        anim.SetTrigger("Attack02"); 
        StopNavAgent(); //공격애니메이션
        StartCoroutine(ResumeMovementAfterAttack()); // 일정 시간 후 이동 다시 시작
    }

    IEnumerator ResumeMovementAfterAttack()
    {
        yield return new WaitForSeconds(1f); // 원하는 대기 시간을 설정

        // 코루틴이 실행되는 동안 navAgent가 비활성화되거나 제거되었는지 확인
        while (navAgent == null || !navAgent.isActiveAndEnabled || !navAgent.isOnNavMesh)
        {
            yield return null;
        }

        navAgent.isStopped = false; // 네비 이동 다시 시작
        anim.SetBool("isMove", true); // isMove를 true로 설정하여 이동 애니메이션 재생
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        StopNavAgent();
        navAgent.enabled = false;
        Invoke("DestroyEnemy", 1f);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject); // 적 오브젝트 제거
    }
    void StopNavAgent() //네비 멈추기
    {
        if (navAgent != null && navAgent.isOnNavMesh && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true; //네비 멈추기
            anim.SetBool("isMove", false); // 이동 애니메이션 멈춤
        }
    }

    public void GetHit(int damage)
    {
        HP -= damage;
        Debug.Log("Enemy HP : " + HP);
        if (HP <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slime")) GetHit(other.GetComponent<Slime>().Attack);
    }
}
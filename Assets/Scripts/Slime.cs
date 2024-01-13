using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    //체력, 피격,방어력(예정)
    public int HP = 10; //유닛 체력
    public float damageInterval = 1f; // 데미지를 받을 주기
    private float nextDamageTime; //다음 데미지를 받을 타이밍
    bool isDead = false;

    //이동용
    public float detectionRadius = 10f; //적 감지 반경
    private float detectionInterval = 0.5f;  // 범위 탐지 주기
    private float sinceLastDetectionTime = 0f; // 탐지 주기 초기화용
    private Transform target; // current target 적
    public Transform enemyCastle; //적 기지 위치. 적 기지> 프리팹>슬라임 프리팹에 연결, Revert>> 스폰 슬라임의 null 오류 해결

    //공격, 공격력(예정)
    public float attackDistance = 3f; // 공격 가능 거리
    public float attackInterval = 1f; //다음 공격 주기
    private float nextAttackTime; //공격주기 누적 초기화용

    //컴포넌트들
    private Animator animator;
    private NavMeshAgent navAgent;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navAgent.enabled = true;
        navAgent.isStopped = false;
    }
    
    private void Start()
    {
        //게임오브젝트 중 적군 성 태그를 가진 오브젝트의 트랜스폼을 향해 가도록 함
        
        if (enemyCastle != null)
        {
            target = enemyCastle.transform;  //타겟에 넣기
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
        if (isDead) return; //죽었으면 아래로는 실행하지 않기

        sinceLastDetectionTime += Time.deltaTime; //시간흐름 저장으로 최적화
        if (sinceLastDetectionTime >= detectionInterval) //범위스캔 간격보다 시간 흐름이 크면
        {
            SearchEnemyInDetection(); //범위 스캔
            sinceLastDetectionTime = 0f; //시간 초기화
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
                animator.SetBool("isMove", false); //idle 애니메이션 실행
            }
            else
            {
                animator.SetBool("isMove", true); //이동(idle2) 애니메이션 실행
            }
        }
    }

    void SearchEnemyInDetection() //범위 스캔
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius); //범위 콜리더 감지, 배열 저장
        Transform closestEnemy = FindClosestEnemy(hitColliders); //가까운 적의 위치 저장
        
        if (closestEnemy != null)
        {
            target = closestEnemy; //가까운 적의 위치로 이동
        }
        else
        {
            target = enemyCastle.transform; //적군 성으로 이동
        }
    }
    Transform FindClosestEnemy(Collider[] colliders) 
    {
        Transform closestEnemy = null; //가장 가까운 적의 위치
        float closestDistance = Mathf.Infinity; //가장 가까운적의 거리

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy")) //적 태그면
            {
                //현재 슬라임과 적의 거리 계산
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance) //적 거리가 가장 가까운적과의 간격보다 작으면
                {
                    closestDistance = distanceToEnemy; //적 거리를 가장 가까운 적과의 간격에 넣기
                    closestEnemy = col.transform; //콜리더의 위치를 가장 가까운 적의 위치에 넣기
                }
            }
        }

        return closestEnemy; //가장 가까운 적의위치를 반환
    }

    private void MoveToTarget(Transform target) //타겟의 위치로 이동
    {
        navAgent.SetDestination(target.position); //네비메쉬를 통해 이동 
    }
 
    private void OnTriggerEnter(Collider other) //물리 충돌로 인한 밀려남 방지를 위한 트리거
    {
        SlimeCollision(other); 
    }
    private void OnTriggerStay(Collider other) //지속적인 트리거 감지중일때도 일정하게 발생하기위함
    {
        SlimeCollision(other);
    }
   
    void SlimeCollision(Collider other)
    {/*
        if (other.gameObject.CompareTag("EnemyCastle") && Time.time >= nextDamageTime) 
        {
            nextDamageTime = Time.time + damageInterval;
        }*/
        if ((other.gameObject.CompareTag("Enemy")) && Time.time >= nextDamageTime)//다음 데미지타임일때만
        {
            GetHit(1); //체력 감소
            nextDamageTime = Time.time + damageInterval; //다음데미지 시간 누적초기화
        }
    }
    void Attack()//공격
    {
        animator.SetTrigger("Attack01"); StopNavAgent(); //공격애니메이션
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
        animator.SetBool("isMove", true); // isMove를 true로 설정하여 이동 애니메이션 재생
    }
    void GetHit(int damage) //데미지를 받음
    {
        HP -= damage; //받을 데미지량만큼 감소
        Debug.Log("Slime HP : " + HP); //콘솔창에 출력

        if (HP <= 0)
        {
            isDead = true; //슬라임은 죽음
            StopNavAgent();  //네비 멈추기
            navAgent.enabled = false; // Agent끄기. StopNavAgent()으로 이동시키면 이동하지않는 문제 발생
            animator.SetTrigger("Death");//사망 애니메이션 재생
            Invoke("Die", 1);//사망애니메이션을 보기위한 시간차
        }
    }

    void StopNavAgent() //네비 멈추기
    {
        if (navAgent != null && navAgent.isOnNavMesh && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true; //네비 멈추기
            animator.SetBool("isMove", false); // 이동 애니메이션 멈춤
        }
    }
    void Die() //사망
    {
        Destroy(gameObject); //오브젝트 삭제
    }

}
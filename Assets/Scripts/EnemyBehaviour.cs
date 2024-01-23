using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    //컴포넌트들
    private Animator anim;
    private NavMeshAgent navAgent;

    private Transform target; // current target 적
    public Transform slimeCastle; //적 기지 위치. 적 기지> 프리팹>슬라임 프리팹에 연결, Revert>> 스폰 슬라임의 null 오류 해결


    [Header("Basic Data")]
    bool isDead = false;
    public float HP = 200f; //유닛 체력
    public float attackDamage = 10f; 
    public float defense = 10f; //
    public float attackSpeed = 1.5f; // 
    public float attackDistance = 3f; // 공격 가능 거리
    public float attackInterval = 1f; //다음 공격 주기
    public float currentHP;

    [Header("Addictional Data")]
    private float nextAttackTime; //공격주기 누적 초기화용
    public float detectionRadius = 10f; //적 감지 반경
    private float detectionInterval = 0.5f;  // 범위 탐지 주기
    private float sinceLastDetectionTime = 0f; // 탐지 주기 초기화용

    [Header("Weapon")]
    public Collider weaponCollider;
    public EnemyWeapon enemyWeapon;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navAgent.enabled = true;
        navAgent.isStopped = false;

        slimeCastle = GameObject.FindWithTag("SlimeCastle").transform;
    }

    private void Start()
    {
        currentHP = HP;

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

        enemyWeapon.weaponDamage = attackDamage;
    }
    void Update()
    {
        if (isDead) return; //죽었으면 아래로는 실행하지 않기

        sinceLastDetectionTime += Time.deltaTime; //시간흐름 저장으로 최적화
        if (sinceLastDetectionTime >= detectionInterval) //범위스캔 간격보다 시간 흐름이 크면
        {
            SearchSlimeInDetection(); //범위 스캔
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

    void SearchSlimeInDetection() //범위 스캔
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius); //범위 콜리더 감지, 배열 저장
        Transform closestEnemy = FindClosestSlime(hitColliders); //가까운 적의 위치 저장

        if (closestEnemy != null)
        {
            target = closestEnemy; 
        }
        else
        {
            target = slimeCastle.transform; 
        }
    }
    Transform FindClosestSlime(Collider[] colliders)
    {
        Transform closestSlime = null; 
        float closestDistance = Mathf.Infinity; 

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Slime")) 
            {

                float distanceToSlime = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToSlime < closestDistance) 
                {
                    closestDistance = distanceToSlime; 
                    closestSlime = col.transform; 
                }
            }
        }

        return closestSlime; //가장 가까운 적의위치를 반환
    }

    private void MoveToTarget(Transform target) //타겟의 위치로 이동
    {
        navAgent.SetDestination(target.position); //네비메쉬를 통해 이동 
    }

    void Attack()//공격
    {
        anim.SetTrigger("Attack02");
        StopNavAgent(); //공격애니메이션
        StartCoroutine(ResumeMovementAfterAttack()); // 일정 시간 후 이동 다시 시작
        StartCoroutine(ActivateWeaponCollider()); // weaponCollider 활성화 코루틴 시작
    }
    IEnumerator ResumeMovementAfterAttack()
    {
        yield return new WaitForSeconds(2f); // 원하는 대기 시간을 설정

        // 코루틴이 실행되는 동안 navAgent가 비활성화되거나 제거되었는지 확인
        while (navAgent == null || !navAgent.isActiveAndEnabled || !navAgent.isOnNavMesh)
        {
            yield return null;
        }

        navAgent.isStopped = false; // 네비 이동 다시 시작
        anim.SetBool("isMove", true); // isMove를 true로 설정하여 이동 애니메이션 재생
    }
    IEnumerator ActivateWeaponCollider()
    {
        weaponCollider.enabled = true; // weaponCollider를 활성화
        yield return new WaitForSeconds(0.5f); // 0.5초 대기
        weaponCollider.enabled = false; // weaponCollider를 다시 비활성화
    }

    public void GetHit(float damage) //데미지를 받음
    {
        currentHP -= damage; //받을 데미지량만큼 감소
        Debug.Log("Enemy HP : " + currentHP); //콘솔창에 출력

        if (currentHP <= 0)
        {
            isDead = true; 
            StopNavAgent();  //네비 멈추기
            navAgent.enabled = false; // Agent끄기. StopNavAgent()으로 이동시키면 이동하지않는 문제 발생
            anim.SetTrigger("Die");//사망 애니메이션 재생
            Invoke("Die", 1);//사망애니메이션을 보기위한 시간차
        }
    }

    void StopNavAgent() //네비 멈추기
    {
        if (navAgent != null && navAgent.isOnNavMesh && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true; //네비 멈추기
            anim.SetBool("isMove", false); // 이동 애니메이션 멈춤
        }
    }
    void Die() //사망
    {
        Destroy(gameObject); //오브젝트 삭제
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Slime") || other.transform.CompareTag("SlimeCastle"))
        {
            weaponCollider.enabled = false;
        }
        if (other.transform.CompareTag("SlimeWeapon"))
        {
            GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
        }
    }

}
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum MeleeSlimeType
{
    NonSkill,
    Epic,
    Legend
}

public class MeleeSlimeBehaviour : MonoBehaviour, ISlime
{
    //컴포넌트들
    private Animator anim;
    private NavMeshAgent navAgent;

    public Transform target; // current target 적
    public Transform enemyCastle; //적 기지 위치. 적 기지> 프리팹>슬라임 프리팹에 연결, Revert>> 스폰 슬라임의 null 오류 해결

    // Slime 정보를 저장할 변수들
    public Slime slimeData; // Slime 스크립트를 연결할 변수


    [Header("Basic Data")]
    bool isDead = false;

    public float MaxHP { get; set; }
    public float AttackDamage { get; set; }
    public float CurrentHP { get; set; }

    public float defense; // Slime의 방어력
    public float attackSpeed; // Slime의 공격 속도
    public float attackDistance = 8f; // 공격 가능 거리
    public float attackInterval = 1.8f; //다음 공격 주기

    [Header("Addictional Data")]
    private float nextAttackTime; //공격주기 누적 초기화용
    public float detectionRadius = 8f; //적 감지 반경
    private float detectionInterval = 0.5f;  // 범위 탐지 주기
    private float sinceLastDetectionTime = 0f; // 탐지 주기 초기화용
    private bool hasAttacked = false;

    [Header("Weapon")]
    public Collider[] weaponColliders;
    public SlimeWeapon slimeWeapon;

    [Header("Melee")]
    public bool isFire = false;
    public bool isSkill = false;
    public MeleeSlimeType meleeSlimeType;
    public GameObject epicStarHit; 
    public GameObject legendStarHit; 

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navAgent.enabled = true;
        navAgent.isStopped = false;

        //슬라임 수치 가져오기
        string slimePrefabName = gameObject.name.Replace("(Clone)", ""); // 여기는 이름 바꿔서 들어오기가 안된다. 
        // Instantiate로 생성됐기에 Awake()가 실행된다음에 이름을 바꾸는것은 틀리다.

        //Slime slimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == slimePrefabName);

        /*
         if (slimeData != null)
        {
            //slimeCost = slimeData.Cost;
            HP = slimeData.HP;
            attackDamage = slimeData.Attack;
            defense = slimeData.Defense;
            attackSpeed = slimeData.AttackSpeed;
        }
        else
        {
            Debug.LogError("Slime data not found for " + slimePrefabName);
        }
         */


        //enemyCastle = GameObject.FindWithTag("EnemyCastle").transform;
    }

    private void Start()
    {
        //게임오브젝트 중 적군 성 태그를 가진 오브젝트의 트랜스폼을 향해 가도록 함
        CurrentHP = MaxHP;
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
        //slimeWeapon.weaponDamage = attackDamage;
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

            if (!isFire) MoveToTarget(target); //타겟을향해 네비메쉬 이동

            float distanceToTarget = Vector3.Distance(transform.position, target.position); //타겟과의 간격계산
            if (distanceToTarget <= attackDistance) //공격범위 이하의 간격이면
            {
                isFire = true;
                navAgent.velocity = new Vector3(0, 0, 0);

                if (Time.time >= nextAttackTime)//공격 쿨타임에 맞춰서 
                {
                    if (isSkill)
                    {
                        isSkill = false;
                        MeleeSkill();
                    }
                    else
                    {
                        Attack(); //공격, 애니메이션이 주기적으로 나오게 하기 위함
                        nextAttackTime = Time.time + attackInterval; //공격 쿨타임 누적 초기화용
                    }
                    
                }
            }
            else
            {
                isFire = false;
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


    void Attack()//공격
    {
        anim.SetTrigger("Attack01");
        StopNavAgent();
        StartCoroutine(ResumeMovementAfterAttack());
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
        foreach(Collider weaponCollider in weaponColliders)
            weaponCollider.enabled = true; // weaponCollider를 활성화

        yield return new WaitForSeconds(0.5f); // 0.5초 대기

        foreach (Collider weaponCollider in weaponColliders)
            weaponCollider.enabled = false; // weaponCollider를 다시 비활성화
    }

    public void GetHit(float damage) //데미지를 받음
    {
        // 실제 대미지 계산: 공격력 - (방어력 * 0.5)
        float actualDamage = damage - (defense * 0.5f);
        // 실제 대미지가 0보다 작으면, 0으로 처리하여 데미지가 없게 함
        actualDamage = Mathf.Max(actualDamage, 0);
        CurrentHP -= actualDamage; //받을 데미지량만큼 감소

        Debug.Log("Slime HP : " + CurrentHP);

        if (CurrentHP <= 0)
        {
            isDead = true; //슬라임은 죽음
            StopNavAgent();  //네비 멈추기
            navAgent.enabled = false; // Agent끄기. StopNavAgent()으로 이동시키면 이동하지않는 문제 발생
            anim.SetTrigger("Death");//사망 애니메이션 재생
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
        if (other.transform.CompareTag("EnemyWeapon"))
        {
            GetHit(other.gameObject.GetComponent<EnemyWeapon>().weaponDamage);
        }
    }

    public void OnSkill()
    {
        isSkill = true;
    }

    public void MeleeSkill()  //여기가 1번째 
    {
        switch(meleeSlimeType)
        {
            case MeleeSlimeType.Epic:
                EpicMeleeSkill();
                break;
            case MeleeSlimeType.Legend:
                LegendMeleeSkill();
                break;
            case MeleeSlimeType.NonSkill:
                break;
        }
    }

    public void EpicMeleeSkill()
    {
        anim.SetTrigger("Skill");
        if (target != null && isFire) // 타겟이 설정되어 있는 경우에만 실행
        {
            // 타겟의 위치에 이펙트 생성
            GameObject effectInstance = Instantiate(epicStarHit, target.position, Quaternion.identity);
            // target이 EnemyBehaviour 컴포넌트를 가지고 있는지 확인
            EnemyBehaviour enemy = target.GetComponent<EnemyBehaviour>();
            EnemyCastle enemyCastle = target.GetComponent<EnemyCastle>();
            if (enemy != null)
            {
                enemy.currentHP -= AttackDamage * 1.5f; // 대미지 적용
            }
            else if(enemyCastle != null)
            {
                enemyCastle.currentHP -= AttackDamage * 1.5f;
            }
            

            // 이펙트를 일정 시간 후에 제거
            Destroy(effectInstance, 2.0f); // 예를 들어, 2초 후에 이펙트 제거
        }
    }
    public void LegendMeleeSkill()
    {
        anim.SetTrigger("Skill");
        if (target != null && isFire) // 타겟이 설정되어 있는 경우에만 실행
        {
            // 타겟의 위치에 이펙트 생성
            GameObject effectInstance = Instantiate(epicStarHit, target.position, Quaternion.identity);
            // target이 EnemyBehaviour 컴포넌트를 가지고 있는지 확인
            EnemyBehaviour enemy = target.GetComponent<EnemyBehaviour>();
            EnemyCastle enemyCastle = target.GetComponent<EnemyCastle>();
            if (enemy != null)
            {
                enemy.currentHP -= AttackDamage * 1.5f; // 대미지 적용
            }
            else if (enemyCastle != null)
            {
                enemyCastle.currentHP -= AttackDamage * 1.5f;
            }


            // 이펙트를 일정 시간 후에 제거
            Destroy(effectInstance, 2.0f); // 예를 들어, 2초 후에 이펙트 제거
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyBehaviour : MonoBehaviour, IEnemy
{
    //컴포넌트들
    private Animator anim;
    private NavMeshAgent navAgent;

    private Transform target; 
    public Transform slimeCastle; 

    [Header("Basic Data")]
    bool isDead = false;
    [field: SerializeField]
    public float MaxHP { get; set; }
    [field: SerializeField]
    public float AttackDamage { get; set; }
    [field: SerializeField]
    public float CurrentHP { get; set; }
    [field: SerializeField]
    public float Defense { get; set; }
    [field: SerializeField]
    public float AttackSpeed { get; set; }
    [field: SerializeField]
    public float MoveSpeed { get; set; }
    [field: SerializeField]
    public float AttackRange { get; set; }
    public bool IsSkill { get; set; }

    [field: SerializeField]
    public float DropJellyPower { get; set; }

    [Header("Addictional Data")]
    private float nextAttackTime; //공격주기 누적 초기화용
    private float detectionRadius = 20f; //적 감지 반경
    private float detectionInterval = 0.5f;  // 범위 탐지 주기
    private float sinceLastDetectionTime = 0f; // 탐지 주기 초기화용


    [Header("Weapon")]
    public Collider weaponCollider;
    public EnemyWeapon enemyWeapon;
    public bool isFire = false;

    [Header("Stun")]
    private bool isStunned = false; // 스턴 상태 관리 변수

    public ClassType classType;
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navAgent.enabled = true;
        navAgent.isStopped = false;

        string enemyPrefabName = gameObject.name.Replace("(Clone)", "");

        Enemy enemyData = GoogleSheetManager.Instance.enemys.FirstOrDefault(enemy => enemy.Name == enemyPrefabName);

        if (enemyData != null)
        {
            //slimeCost = slimeData.Cost;
            MaxHP = enemyData.HP;
            AttackDamage = enemyData.AttackDamage;
            Defense = enemyData.Defense;
            AttackSpeed = enemyData.AttackSpeed;
            AttackRange = enemyData.AttackRange;
            DropJellyPower = enemyData.DropJellyPower;
            navAgent.speed = enemyData.MoveSpeed;
        }
        else
        {
            Debug.LogError("Enemy data not found for " + enemyPrefabName);
        }

        slimeCastle = GameObject.FindWithTag("SlimeCastle").transform;
    }

    private void Start()
    {
        //게임오브젝트 중 적군 성 태그를 가진 오브젝트의 트랜스폼을 향해 가도록 함
        CurrentHP = MaxHP;
        if (slimeCastle != null)
        {
            target = slimeCastle.transform;  //타겟에 넣기
            MoveToTarget(target);// 시작 시 적군 성으로 이동
        }
        else
        {
            Debug.LogError("SlimeCastle not found in the scene.");
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

        enemyWeapon.weaponDamage = AttackDamage;
        enemyWeapon.classType = classType;
    }
    void Update()
    {
        if (isDead || isStunned) return; // 죽었거나 스턴 상태이면 아래 로직 실행 안 함

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
            if (distanceToTarget <= AttackRange) //공격범위 이하의 간격이면
            {
                isFire = true;
                navAgent.velocity = new Vector3(0, 0, 0);

                if (Time.time >= nextAttackTime)//공격 쿨타임에 맞춰서 
                {
                    Attack(); //공격, 애니메이션이 주기적으로 나오게 하기 위함
                    nextAttackTime = Time.time + AttackSpeed; //공격 쿨타임 누적 초기화용
                }
            }

        }
        float currentVelocity = navAgent.velocity.magnitude;// 움직임 여부를 판단

        if (currentVelocity <= 0.5f)
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
        Transform closestSlime = FindClosestSlime(hitColliders); //가까운 적의 위치 저장

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
        Transform closestEnemy = null; //가장 가까운 적의 위치
        float closestDistance = Mathf.Infinity; //가장 가까운적의 거리

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Slime")) //적 태그면
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
        float randomValue = Random.Range(0f, 1f);

        // Use the random number to determine the attack animation
        if (randomValue < 0.5f)
        {
            anim.SetTrigger("Attack01");
        }
        else
        {
            anim.SetTrigger("Attack02");
        }

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
        yield return new WaitForSeconds(0.2f);
        weaponCollider.enabled = true; // weaponCollider를 활성화
        yield return new WaitForSeconds(0.5f); // 0.5초 대기
        weaponCollider.enabled = false; // weaponCollider를 다시 비활성화
    }

    public void GetHit(float damage) //데미지를 받음
    {
        // 실제 대미지 계산: 공격력 - (방어력 * 0.5)
        float actualDamage = damage - (Defense * 0.5f);
        // 실제 대미지가 0보다 작으면, 0으로 처리하여 데미지가 없게 함
        actualDamage = Mathf.Max(actualDamage, 0);
        CurrentHP -= actualDamage; //받을 데미지량만큼 감소

        Debug.Log("Enemy : " + CurrentHP);

        if (CurrentHP <= 0)
        {
            isDead = true; 
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
        SlimeSpawnManager.instance.jellyPower += DropJellyPower;

        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_EnemyDeathSound);

        Destroy(gameObject); //오브젝트 삭제
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Slime") || other.gameObject.CompareTag("SlimeCastle")) weaponCollider.enabled = false;

        if (other.gameObject.CompareTag("SlimeWeapon"))
        {
            if(ClassType.Tanker == classType)  // 몬스터가 탱커
            {
                if(other.gameObject.GetComponent<SlimeWeapon>().classType == ClassType.Ranged)
                {
                    GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage * 0.7f);
                }
                else if(other.gameObject.GetComponent<SlimeWeapon>().classType == ClassType.Melee)
                {
                    GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage * 1.3f);
                }
                else
                {
                    GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
                }
            }
            else if(ClassType.Melee == classType) // 몬스터가 근접
            {
                if (other.gameObject.GetComponent<SlimeWeapon>().classType == ClassType.Ranged) // 원거리면
                {
                    GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage * 1.3f);
                }
                else
                {
                    GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
                }
            }
            else
            {
                GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
            }


        }
        else if (other.gameObject.CompareTag("SlimeProjectileWeapon"))
        {
            SlimeWeapon slimeWeapon = other.gameObject.GetComponent<SlimeWeapon>();
            if (slimeWeapon != null)
            {
                if (ClassType.Tanker == classType)  // 몬스터가 탱커
                {
                    if (other.gameObject.GetComponent<SlimeWeapon>().classType == ClassType.Ranged)
                    {
                        GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage * 0.7f);
                    }
                    else if (other.gameObject.GetComponent<SlimeWeapon>().classType == ClassType.Melee)
                    {
                        GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage * 1.3f);
                    }
                }
                else if (ClassType.Melee == classType) // 몬스터가 근접
                {
                    if (other.gameObject.GetComponent<SlimeWeapon>().classType == ClassType.Ranged) // 원거리면
                    {
                        GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage * 1.3f);
                    }
                }
                else
                {
                    GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
                }
                Destroy(other.gameObject);
            }

        }
        else if (other.gameObject.CompareTag("Meteor"))
        {
            GetStunned(5f);
        }
        else
        {
            return;
        }
    }

    public void GetStunned(float duration)
    {
        if (!isStunned) // 이미 스턴 상태가 아니라면
        {
            
            StartCoroutine(StunDuration(duration));
        }
    }

    IEnumerator StunDuration(float duration)
    {
        GameObject stunStar = Instantiate(PlayerSkillManager.instance.stunStarEffect, transform);
        isStunned = true; // 스턴 상태로 전환
        navAgent.isStopped = true; // 몬스터 이동 중지

        yield return new WaitForSeconds(duration); // 스턴 지속 시간 대기

        if (!isDead) // 스턴 종료 후, 몬스터가 살아있다면
        {
            Destroy(stunStar);
            isStunned = false; // 스턴 상태 해제
            navAgent.isStopped = false; // 몬스터 이동 재개
        }
    }
}

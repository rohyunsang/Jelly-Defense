using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform slimeCastle;
    private NavMeshAgent navAgent;
    private Animator anim;

    public float detectionRadius = 10f; // 감지 반경
    public float attackDistance = 1f; // 공격 가능 거리
    private Transform target; // current target 슬라임, 적 기지

    private float detectionInterval = 0.5f;  // 
    private float sinceLastDetectionTime = 0f; // 

    public int HP = 100;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = slimeCastle; // 시작 타겟 슬라임 성
        MoveToTarget(target); // 시작 시 슬라임 성으로 이동
    }

    void Update()
    {
        sinceLastDetectionTime += Time.deltaTime;
        if (sinceLastDetectionTime >= detectionInterval)
        {
            SearchSlimeInDetection();
            sinceLastDetectionTime = 0f;
        }

        if (target != null)
        {
            navAgent.SetDestination(target.position);
            anim.SetBool("isMove", true);

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackDistance)
            {
                Attack();
            }
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }

    void SearchSlimeInDetection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        if (hitColliders == null)
            MoveToTarget(slimeCastle);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Slime"))
            {
                target = hitCollider.transform;
                return; // 슬라임 발견 시 즉시 리턴
            }
        }
        MoveToTarget(target);
        
    }
    void MoveToTarget(Transform targetTransform)
    {
        navAgent.SetDestination(targetTransform.position); // Move to the specified target
    }

    void Attack()
    {
        if (navAgent.enabled)
        {
            navAgent.isStopped = true; // Stop the navAgent while attacking
        }
        anim.SetTrigger("Attack01"); 
        // 공격 로직 추가해야함.
    }
    void Die()
    {
        anim.SetTrigger("Die");
        Destroy(gameObject); // 적 오브젝트 제거
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
    }
}

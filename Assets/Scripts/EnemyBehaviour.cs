using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform slimeCastle;
    private NavMeshAgent navAgent;
    private Animator anim;

    public float detectionRadius = 10f; // ���� �ݰ�
    public float attackDistance = 1f; // ���� ���� �Ÿ�
    private Transform target; // current target ������, �� ����

    private float detectionInterval = 0.5f;  // 
    private float sinceLastDetectionTime = 0f; // 

    public int HP = 100;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = slimeCastle; // ���� Ÿ�� ������ ��
        MoveToTarget(target); // ���� �� ������ ������ �̵�
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
                return; // ������ �߰� �� ��� ����
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
        // ���� ���� �߰��ؾ���.
    }
    void Die()
    {
        anim.SetTrigger("Die");
        Destroy(gameObject); // �� ������Ʈ ����
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

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    public int HP = 10; //�Ʊ� ���� ü��
    public float detectionRadius = 10f; //�Ʊ� ������ �� ���� �ݰ�
    public float damageInterval = 1f; // �������� ���� �ֱ�
    private float nextDamageTime; //���� �������� ���� Ÿ�̹�

    private Animator animator;
    private NavMeshAgent navAgent;
    private Transform enemyCastle; //������ ���� null�� �Ǵ� ���� ������ ���� private�� ����

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
        //���ӿ�����Ʈ �� ���� �� �±׸� ���� ������Ʈ�� Ʈ�������� ���� ������ ��
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
    
        // NavMeshAgent�� �ʱ�ȭ�� �� NavMesh�� ����Ǿ� �־�� �մϴ�.
        if (navAgent.isOnNavMesh)
        {
            // NavMeshAgent�� Ȱ��ȭ�ϰ� �ʱ� ��ġ�� �̵�
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


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius); //���� �ݸ��� ����
        Transform closestEnemy = FindClosestEnemy(hitColliders); //����� ���� ��ġ

        if (closestEnemy != null)
        {
            // ���� ����� ���� ���� �̵� //#�̰� ���� �����ٰ��� �ֺ��� ���Ͱ� �����Ǹ� ���͸� �����ؾ���.
            navAgent.SetDestination(closestEnemy.position);
        }
        else
        {
            // Enemy �±׸� ���� ������Ʈ �������� ������ EnemyCastle �±׸� ���� ������Ʈ�� ����
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
        Transform closestEnemy = null; //���� ����� ���� ��ġ
        float closestDistance = Mathf.Infinity; //���� ��������� �Ÿ�

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy")) //�� �±׸�
            {//���� �����Ӱ� ���� �Ÿ� ���
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

    private void OnTriggerEnter(Collider other) //���� �浹�� ���� �з��� ����
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
            //# ���⼭ ���� �� �ٸ��ƾߵ� 
            //# rigidbody���� 
            StopNavAgent(); //# ������ �����ִϱ� ���絵 ���ƿ� 
            navAgent.enabled = false; //# NavMeshAgent����
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
    void Die() //# �ٽ� ������ Die() 
    {
        Destroy(gameObject);
    }

}
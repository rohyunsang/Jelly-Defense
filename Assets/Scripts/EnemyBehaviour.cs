using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform slimeCastle; // ������ �� ��ġ
    private NavMeshAgent navAgent;
    private Animator anim;

    //�ǰ�(���� �׽��ÿ�)
    public float damageInterval = 1f; // �������� ���� �ֱ�
    private float nextDamageTime; //���� �������� ���� Ÿ�̹�
    

    //�̵���
    public float detectionRadius = 10f; //�� ���� �ݰ�
    private float detectionInterval = 0.5f;  // ���� Ž�� �ֱ�
    private float sinceLastDetectionTime = 0f; // Ž�� �ֱ� �ʱ�ȭ��
    private Transform target; // current target ��

    //����, ���ݷ�(����)
    public float attackDistance = 3f; // ���� ���� �Ÿ�
    public float attackInterval = 1f; //���� ���� �ֱ�
    private float nextAttackTime; //�����ֱ� ���� �ʱ�ȭ��

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
        //���ӿ�����Ʈ �� ���� �� �±׸� ���� ������Ʈ�� Ʈ�������� ���� ������ ��

        if (slimeCastle != null)
        {
            target = slimeCastle.transform;  //Ÿ�ٿ� �ֱ�
            MoveToTarget(target);// ���� �� ���� ������ �̵�
        }
        else
        {
            Debug.LogError("EnemyCastle not found in the scene.");
        }

        // NavMeshAgent�� �ʱ�ȭ�� �� NavMesh�� ����Ǿ� �־�� ��. ������ �����߻�
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

        sinceLastDetectionTime += Time.deltaTime;
        if (sinceLastDetectionTime >= detectionInterval)
        {
            SearchSlimeInDetection();
            sinceLastDetectionTime = 0f;
        }

        if (target != null) //Ÿ���� ������
        {
            MoveToTarget(target); //Ÿ�������� �׺�޽� �̵�

            float distanceToTarget = Vector3.Distance(transform.position, target.position); //Ÿ�ٰ��� ���ݰ��
            if (distanceToTarget <= attackDistance) //���ݹ��� ������ �����̸�
            {
                if (Time.time >= nextAttackTime)//���� ��Ÿ�ӿ� ���缭 
                {
                    Attack(); //����, �ִϸ��̼��� �ֱ������� ������ �ϱ� ����
                    nextAttackTime = Time.time + attackInterval; //���� ��Ÿ�� ���� �ʱ�ȭ��
                }
            }
        }
        else
        {
            float currentVelocity = navAgent.velocity.magnitude;// ������ ���θ� �Ǵ�
            if (currentVelocity <= 1f)
            {
                anim.SetBool("isMove", false); //idle �ִϸ��̼� ����
            }
            else
            {
                anim.SetBool("isMove", true); //�̵�(idle2) �ִϸ��̼� ����
            }
        }
    }

    void SearchSlimeInDetection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius); //���� �ݸ��� ����, �迭 ����
        Transform closestSlime = FindClosestSlime(hitColliders); //����� �������� ��ġ ����

        if (closestSlime != null)
        {
            target = closestSlime; //����� ���� ��ġ�� �̵�
        }
        else
        {
            target = slimeCastle.transform; //������ ������ �̵�
        }
    }

    Transform FindClosestSlime(Collider[] colliders)
    {
        Transform closestSlime = null; //���� ����� �������� ��ġ
        float closestDistance = Mathf.Infinity; //���� ��������� �Ÿ�

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Slime")) //������ �±׸�
            {
                //���� ���� �������� �Ÿ� ���
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance) //�� �Ÿ��� ���� ����������� ���ݺ��� ������
                {
                    closestDistance = distanceToEnemy; //�� �Ÿ��� ���� ����� ������ ���ݿ� �ֱ�
                    closestSlime = col.transform; //�ݸ����� ��ġ�� ���� ����� ���� ��ġ�� �ֱ�
                }
            }
        }

        return closestSlime; //���� ����� ������ġ�� ��ȯ
    }

    void MoveToTarget(Transform target)
    {
        navAgent.SetDestination(target.position); // ������ Ÿ������ �̵�
    }

    void Attack()
    {
        anim.SetTrigger("Attack02"); 
        StopNavAgent(); //���ݾִϸ��̼�
        StartCoroutine(ResumeMovementAfterAttack()); // ���� �ð� �� �̵� �ٽ� ����
    }

    IEnumerator ResumeMovementAfterAttack()
    {
        yield return new WaitForSeconds(1f); // ���ϴ� ��� �ð��� ����

        // �ڷ�ƾ�� ����Ǵ� ���� navAgent�� ��Ȱ��ȭ�ǰų� ���ŵǾ����� Ȯ��
        while (navAgent == null || !navAgent.isActiveAndEnabled || !navAgent.isOnNavMesh)
        {
            yield return null;
        }

        navAgent.isStopped = false; // �׺� �̵� �ٽ� ����
        anim.SetBool("isMove", true); // isMove�� true�� �����Ͽ� �̵� �ִϸ��̼� ���
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
        Destroy(gameObject); // �� ������Ʈ ����
    }
    void StopNavAgent() //�׺� ���߱�
    {
        if (navAgent != null && navAgent.isOnNavMesh && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true; //�׺� ���߱�
            anim.SetBool("isMove", false); // �̵� �ִϸ��̼� ����
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
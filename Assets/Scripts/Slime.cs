using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    //ü��, �ǰ�,����(����)
    public int HP = 10; //���� ü��
    public float damageInterval = 1f; // �������� ���� �ֱ�
    private float nextDamageTime; //���� �������� ���� Ÿ�̹�
    bool isDead = false;

    //�̵���
    public float detectionRadius = 10f; //�� ���� �ݰ�
    private float detectionInterval = 0.5f;  // ���� Ž�� �ֱ�
    private float sinceLastDetectionTime = 0f; // Ž�� �ֱ� �ʱ�ȭ��
    private Transform target; // current target ��
    public Transform enemyCastle; //�� ���� ��ġ. �� ����> ������>������ �����տ� ����, Revert>> ���� �������� null ���� �ذ�

    //����, ���ݷ�(����)
    public float attackDistance = 3f; // ���� ���� �Ÿ�
    public float attackInterval = 1f; //���� ���� �ֱ�
    private float nextAttackTime; //�����ֱ� ���� �ʱ�ȭ��

    //������Ʈ��
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
        //���ӿ�����Ʈ �� ���� �� �±׸� ���� ������Ʈ�� Ʈ�������� ���� ������ ��
        
        if (enemyCastle != null)
        {
            target = enemyCastle.transform;  //Ÿ�ٿ� �ֱ�
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
        if (isDead) return; //�׾����� �Ʒ��δ� �������� �ʱ�

        sinceLastDetectionTime += Time.deltaTime; //�ð��帧 �������� ����ȭ
        if (sinceLastDetectionTime >= detectionInterval) //������ĵ ���ݺ��� �ð� �帧�� ũ��
        {
            SearchEnemyInDetection(); //���� ��ĵ
            sinceLastDetectionTime = 0f; //�ð� �ʱ�ȭ
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
                animator.SetBool("isMove", false); //idle �ִϸ��̼� ����
            }
            else
            {
                animator.SetBool("isMove", true); //�̵�(idle2) �ִϸ��̼� ����
            }
        }
    }

    void SearchEnemyInDetection() //���� ��ĵ
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius); //���� �ݸ��� ����, �迭 ����
        Transform closestEnemy = FindClosestEnemy(hitColliders); //����� ���� ��ġ ����
        
        if (closestEnemy != null)
        {
            target = closestEnemy; //����� ���� ��ġ�� �̵�
        }
        else
        {
            target = enemyCastle.transform; //���� ������ �̵�
        }
    }
    Transform FindClosestEnemy(Collider[] colliders) 
    {
        Transform closestEnemy = null; //���� ����� ���� ��ġ
        float closestDistance = Mathf.Infinity; //���� ��������� �Ÿ�

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy")) //�� �±׸�
            {
                //���� �����Ӱ� ���� �Ÿ� ���
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance) //�� �Ÿ��� ���� ����������� ���ݺ��� ������
                {
                    closestDistance = distanceToEnemy; //�� �Ÿ��� ���� ����� ������ ���ݿ� �ֱ�
                    closestEnemy = col.transform; //�ݸ����� ��ġ�� ���� ����� ���� ��ġ�� �ֱ�
                }
            }
        }

        return closestEnemy; //���� ����� ������ġ�� ��ȯ
    }

    private void MoveToTarget(Transform target) //Ÿ���� ��ġ�� �̵�
    {
        navAgent.SetDestination(target.position); //�׺�޽��� ���� �̵� 
    }
 
    private void OnTriggerEnter(Collider other) //���� �浹�� ���� �з��� ������ ���� Ʈ����
    {
        SlimeCollision(other); 
    }
    private void OnTriggerStay(Collider other) //�������� Ʈ���� �������϶��� �����ϰ� �߻��ϱ�����
    {
        SlimeCollision(other);
    }
   
    void SlimeCollision(Collider other)
    {/*
        if (other.gameObject.CompareTag("EnemyCastle") && Time.time >= nextDamageTime) 
        {
            nextDamageTime = Time.time + damageInterval;
        }*/
        if ((other.gameObject.CompareTag("Enemy")) && Time.time >= nextDamageTime)//���� ������Ÿ���϶���
        {
            GetHit(1); //ü�� ����
            nextDamageTime = Time.time + damageInterval; //���������� �ð� �����ʱ�ȭ
        }
    }
    void Attack()//����
    {
        animator.SetTrigger("Attack01"); StopNavAgent(); //���ݾִϸ��̼�
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
        animator.SetBool("isMove", true); // isMove�� true�� �����Ͽ� �̵� �ִϸ��̼� ���
    }
    void GetHit(int damage) //�������� ����
    {
        HP -= damage; //���� ����������ŭ ����
        Debug.Log("Slime HP : " + HP); //�ܼ�â�� ���

        if (HP <= 0)
        {
            isDead = true; //�������� ����
            StopNavAgent();  //�׺� ���߱�
            navAgent.enabled = false; // Agent����. StopNavAgent()���� �̵���Ű�� �̵������ʴ� ���� �߻�
            animator.SetTrigger("Death");//��� �ִϸ��̼� ���
            Invoke("Die", 1);//����ִϸ��̼��� �������� �ð���
        }
    }

    void StopNavAgent() //�׺� ���߱�
    {
        if (navAgent != null && navAgent.isOnNavMesh && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true; //�׺� ���߱�
            animator.SetBool("isMove", false); // �̵� �ִϸ��̼� ����
        }
    }
    void Die() //���
    {
        Destroy(gameObject); //������Ʈ ����
    }

}
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    //������Ʈ��
    private Animator anim;
    private NavMeshAgent navAgent;

    private Transform target; // current target ��
    public Transform slimeCastle; //�� ���� ��ġ. �� ����> ������>������ �����տ� ����, Revert>> ���� �������� null ���� �ذ�


    [Header("Basic Data")]
    bool isDead = false;
    public float HP = 200f; //���� ü��
    public float attackDamage = 10f; 
    public float defense = 10f; //
    public float attackSpeed = 1.5f; // 
    public float attackDistance = 3f; // ���� ���� �Ÿ�
    public float attackInterval = 1f; //���� ���� �ֱ�
    public float currentHP;

    [Header("Addictional Data")]
    private float nextAttackTime; //�����ֱ� ���� �ʱ�ȭ��
    public float detectionRadius = 10f; //�� ���� �ݰ�
    private float detectionInterval = 0.5f;  // ���� Ž�� �ֱ�
    private float sinceLastDetectionTime = 0f; // Ž�� �ֱ� �ʱ�ȭ��

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

        enemyWeapon.weaponDamage = attackDamage;
    }
    void Update()
    {
        if (isDead) return; //�׾����� �Ʒ��δ� �������� �ʱ�

        sinceLastDetectionTime += Time.deltaTime; //�ð��帧 �������� ����ȭ
        if (sinceLastDetectionTime >= detectionInterval) //������ĵ ���ݺ��� �ð� �帧�� ũ��
        {
            SearchSlimeInDetection(); //���� ��ĵ
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

    void SearchSlimeInDetection() //���� ��ĵ
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius); //���� �ݸ��� ����, �迭 ����
        Transform closestEnemy = FindClosestSlime(hitColliders); //����� ���� ��ġ ����

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

        return closestSlime; //���� ����� ������ġ�� ��ȯ
    }

    private void MoveToTarget(Transform target) //Ÿ���� ��ġ�� �̵�
    {
        navAgent.SetDestination(target.position); //�׺�޽��� ���� �̵� 
    }

    void Attack()//����
    {
        anim.SetTrigger("Attack02");
        StopNavAgent(); //���ݾִϸ��̼�
        StartCoroutine(ResumeMovementAfterAttack()); // ���� �ð� �� �̵� �ٽ� ����
        StartCoroutine(ActivateWeaponCollider()); // weaponCollider Ȱ��ȭ �ڷ�ƾ ����
    }
    IEnumerator ResumeMovementAfterAttack()
    {
        yield return new WaitForSeconds(2f); // ���ϴ� ��� �ð��� ����

        // �ڷ�ƾ�� ����Ǵ� ���� navAgent�� ��Ȱ��ȭ�ǰų� ���ŵǾ����� Ȯ��
        while (navAgent == null || !navAgent.isActiveAndEnabled || !navAgent.isOnNavMesh)
        {
            yield return null;
        }

        navAgent.isStopped = false; // �׺� �̵� �ٽ� ����
        anim.SetBool("isMove", true); // isMove�� true�� �����Ͽ� �̵� �ִϸ��̼� ���
    }
    IEnumerator ActivateWeaponCollider()
    {
        weaponCollider.enabled = true; // weaponCollider�� Ȱ��ȭ
        yield return new WaitForSeconds(0.5f); // 0.5�� ���
        weaponCollider.enabled = false; // weaponCollider�� �ٽ� ��Ȱ��ȭ
    }

    public void GetHit(float damage) //�������� ����
    {
        currentHP -= damage; //���� ����������ŭ ����
        Debug.Log("Enemy HP : " + currentHP); //�ܼ�â�� ���

        if (currentHP <= 0)
        {
            isDead = true; 
            StopNavAgent();  //�׺� ���߱�
            navAgent.enabled = false; // Agent����. StopNavAgent()���� �̵���Ű�� �̵������ʴ� ���� �߻�
            anim.SetTrigger("Die");//��� �ִϸ��̼� ���
            Invoke("Die", 1);//����ִϸ��̼��� �������� �ð���
        }
    }

    void StopNavAgent() //�׺� ���߱�
    {
        if (navAgent != null && navAgent.isOnNavMesh && navAgent.isActiveAndEnabled)
        {
            navAgent.isStopped = true; //�׺� ���߱�
            anim.SetBool("isMove", false); // �̵� �ִϸ��̼� ����
        }
    }
    void Die() //���
    {
        Destroy(gameObject); //������Ʈ ����
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
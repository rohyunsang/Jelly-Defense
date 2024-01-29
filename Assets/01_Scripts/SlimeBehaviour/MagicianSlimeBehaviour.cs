using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MagicianSlimeType
{
    NonSkill,
    Epic,
    Legend
}


public class MagicianSlimeBehaviour : MonoBehaviour
{
    //������Ʈ��
    private Animator anim;
    private NavMeshAgent navAgent;

    private Transform target; // current target ��
    public Transform enemyCastle; //�� ���� ��ġ. �� ����> ������>������ �����տ� ����, Revert>> ���� �������� null ���� �ذ�

    // Slime ������ ������ ������
    public Slime slimeData; // Slime ��ũ��Ʈ�� ������ ����


    [Header("Basic Data")]
    bool isDead = false;
    public float HP = 100; //���� ü��
    public float attackDamage; // Slime�� ���ݷ�
    public float defense; // Slime�� ����
    public float attackSpeed; // Slime�� ���� �ӵ�
    public float attackDistance = 8f; // ���� ���� �Ÿ�
    public float attackInterval = 1.8f; //���� ���� �ֱ�
    public float currentHP;

    [Header("Addictional Data")]
    private float nextAttackTime; //�����ֱ� ���� �ʱ�ȭ��
    public float detectionRadius = 20f; //�� ���� �ݰ�
    private float detectionInterval = 0.5f;  // ���� Ž�� �ֱ�
    private float sinceLastDetectionTime = 0f; // Ž�� �ֱ� �ʱ�ȭ��
    private bool hasAttacked = false;

    [Header("Weapon")]
    //public SlimeWeapon slimeWeapon;

    [Header("Magician")]
    public GameObject magicPrefab;
    public GameObject skillPrefab;
    public float magicSpeed = 20f;
    public Transform firePoint;
    public bool isFire = false;
    public bool isSkill;
    public MagicianSlimeType magicianSlimeType;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navAgent.enabled = true;
        navAgent.isStopped = false;

        //������ ��ġ ��������
        string slimePrefabName = gameObject.name.Replace("(Clone)", ""); // ����� �̸� �ٲ㼭 �����Ⱑ �ȵȴ�. 
                                                                         // Instantiate�� �����Ʊ⿡ Awake()�� ����ȴ����� �̸��� �ٲٴ°��� Ʋ����.
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
        //���ӿ�����Ʈ �� ���� �� �±׸� ���� ������Ʈ�� Ʈ�������� ���� ������ ��
        currentHP = HP;
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
        //slimeWeapon.weaponDamage = attackDamage;
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

            if (!isFire) MoveToTarget(target); //Ÿ�������� �׺�޽� �̵�

            float distanceToTarget = Vector3.Distance(transform.position, target.position); //Ÿ�ٰ��� ���ݰ��
            if (distanceToTarget <= attackDistance) //���ݹ��� ������ �����̸�
            {
                isFire = true;
                navAgent.velocity = new Vector3(0, 0, 0);

                if (Time.time >= nextAttackTime)//���� ��Ÿ�ӿ� ���缭 
                {
                    if (isSkill)
                    {
                        isSkill = false;
                        MagicianSkill();
                    }
                    else
                    {
                        Attack(); //����, �ִϸ��̼��� �ֱ������� ������ �ϱ� ����
                    }
                    
                    nextAttackTime = Time.time + attackInterval; //���� ��Ÿ�� ���� �ʱ�ȭ��
                }
            }
            else
            {
                isFire = false;
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

    void MagicArrow(Transform target , GameObject magicPrefab)
    {
        // ȭ�� ���������κ��� ȭ�� ��ü ����
        GameObject arrow = Instantiate(magicPrefab, firePoint.position + new Vector3(0f,1f,0f), firePoint.rotation);
        // ȭ�쿡 Rigidbody ������Ʈ�� �ִ��� Ȯ���ϰ�, ������ �߻�
        arrow.transform.LookAt(target.position);

        // ȭ�쿡 Rigidbody ������Ʈ�� �ִ��� Ȯ���ϰ�, ������ �߻�
        Rigidbody arrowRigid = arrow.GetComponent<Rigidbody>();
        if (arrowRigid != null)
        {
            // Ÿ�� ������ ����մϴ�.
            Vector3 direction = (target.position - firePoint.position).normalized;
            // ȭ�쿡 ���� ���Ͽ� �߻��մϴ�.
            arrowRigid.velocity = direction * magicSpeed;
        }

    }

    void Attack()//����
    {
        MagicArrow(target, magicPrefab);
        //anim.SetTrigger("Attack02");
        StopNavAgent();
        StartCoroutine(ResumeMovementAfterAttack());
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
        //weaponCollider.enabled = true; // weaponCollider�� Ȱ��ȭ
        yield return new WaitForSeconds(0.5f); // 0.5�� ���
        //weaponCollider.enabled = false; // weaponCollider�� �ٽ� ��Ȱ��ȭ
    }

    public void GetHit(float damage) //�������� ����
    {
        // ���� ����� ���: ���ݷ� - (���� * 0.5)
        float actualDamage = damage - (defense * 0.5f);
        // ���� ������� 0���� ������, 0���� ó���Ͽ� �������� ���� ��
        actualDamage = Mathf.Max(actualDamage, 0);
        currentHP -= actualDamage; //���� ����������ŭ ����

        Debug.Log("Slime HP : " + currentHP);

        if (currentHP <= 0)
        {
            isDead = true; //�������� ����
            StopNavAgent();  //�׺� ���߱�
            navAgent.enabled = false; // Agent����. StopNavAgent()���� �̵���Ű�� �̵������ʴ� ���� �߻�
            anim.SetTrigger("Death");//��� �ִϸ��̼� ���
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
        if (other.transform.CompareTag("EnemyWeapon"))
        {
            GetHit(other.gameObject.GetComponent<EnemyWeapon>().weaponDamage);
        }
    }

    public void OnSkill()  //���Ⱑ 1��° 
    {
        isSkill = true;
    }


    private void MagicianSkill()
    {
        switch (magicianSlimeType)
        {
            case MagicianSlimeType.Epic:
                EpicMagicianSkill();
                break;
            case MagicianSlimeType.Legend:
                EpicMagicianSkill();
                break;
            case MagicianSlimeType.NonSkill:
                break;
        }
    }

    public void EpicMagicianSkill()
    {
        MagicArrow(target, skillPrefab);
        StopNavAgent();
        StartCoroutine(ResumeMovementAfterAttack());
        StartCoroutine(ActivateWeaponCollider());
    }
    

}
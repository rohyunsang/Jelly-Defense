using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum MagicianSlimeType
{
    NonSkill,
    Epic,
    Legend,
    LegendHealer
}


public class MagicianSlimeBehaviour : MonoBehaviour, ISlime
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

    [Header("Addictional Data")]
    private float nextAttackTime; //�����ֱ� ���� �ʱ�ȭ��
    private float detectionRadius = 20f; //�� ���� �ݰ�
    private float detectionInterval = 0.5f;  // ���� Ž�� �ֱ�
    private float sinceLastDetectionTime = 0f; // Ž�� �ֱ� �ʱ�ȭ��

    [Header("Weapon")]
    //public SlimeWeapon slimeWeapon;

    [Header("Magician")]
    public GameObject magicPrefab;
    public GameObject skillPrefab;
    public float magicSpeed = 20f;
    public Transform firePoint;
    public bool isFire = false;
    public MagicianSlimeType magicianSlimeType;
    public bool IsSkill { get; set; }


    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navAgent.enabled = true;
        navAgent.isStopped = false;

        //������ ��ġ ��������
        string slimePrefabName = gameObject.name.Replace("(Clone)", ""); // ����� �̸� �ٲ㼭 �����Ⱑ �ȵȴ�. 
                                                                         // Instantiate�� �����Ʊ⿡ Awake()�� ����ȴ����� �̸��� �ٲٴ°��� Ʋ����.
        Slime slimeData = GoogleSheetManager.Instance.slimes.FirstOrDefault(slime => slime.Name == slimePrefabName);

        if (slimeData != null)
        {
            //slimeCost = slimeData.Cost;
            MaxHP = slimeData.HP;
            AttackDamage = slimeData.AttackDamage;
            Defense = slimeData.Defense;
            AttackSpeed = slimeData.AttackSpeed;
            AttackRange = slimeData.AttackRange;
        }
        else
        {
            Debug.LogError("Slime data not found for " + slimePrefabName);
        }



        enemyCastle = GameObject.FindWithTag("EnemyCastle").transform;
        
    }

    private void Start()
    {
        //���ӿ�����Ʈ �� ���� �� �±׸� ���� ������Ʈ�� Ʈ�������� ���� ������ ��
        CurrentHP = MaxHP;
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
        magicPrefab.GetComponent<SlimeWeapon>().weaponDamage = AttackDamage;
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
            if (distanceToTarget <= AttackRange) //���ݹ��� ������ �����̸�
            {
                isFire = true;
                navAgent.velocity = new Vector3(0, 0, 0);

                if (Time.time >= nextAttackTime)//���� ��Ÿ�ӿ� ���缭 
                {
                    if (IsSkill)
                    {
                        IsSkill = false;
                        MagicianSkill();
                    }
                    else
                    {
                        Attack(); //����, �ִϸ��̼��� �ֱ������� ������ �ϱ� ����
                    }
                    
                    nextAttackTime = Time.time + AttackSpeed; //���� ��Ÿ�� ���� �ʱ�ȭ��
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

    public void SlimeWeaponDamageUpdate()
    {
        magicPrefab.GetComponent<SlimeWeapon>().weaponDamage = AttackDamage;
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
        GameObject arrow = Instantiate(magicPrefab, firePoint.position, firePoint.rotation);

        Destroy(arrow, 3f);
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
        float actualDamage = damage - (Defense * 0.5f);
        // ���� ������� 0���� ������, 0���� ó���Ͽ� �������� ���� ��
        actualDamage = Mathf.Max(actualDamage, 0);
        CurrentHP -= actualDamage; //���� ����������ŭ ����

        Debug.Log("Slime HP : " + CurrentHP);

        if (CurrentHP <= 0)
        {
            isDead = true; //�������� ����
            StopNavAgent();  //�׺� ���߱�
            navAgent.enabled = false; // Agent����. StopNavAgent()���� �̵���Ű�� �̵������ʴ� ���� �߻�
            anim.SetTrigger("Death");//��� �ִϸ��̼� ���

            // ������ ������ �ʱ�ȭ �κ�.
            if (MagicianSlimeType.Legend == magicianSlimeType)
                SlimeSpawnManager.instance.DieLegendSlime();


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
        else if (other.transform.CompareTag("EnemyProjectileWeapon"))
        {
            EnemyWeapon enemyWeapon = other.gameObject.GetComponent<EnemyWeapon>();
            if (enemyWeapon != null)
            {
                GetHit(other.gameObject.GetComponent<EnemyWeapon>().weaponDamage);
                Destroy(other.gameObject);
            }

        }
        else
        {
            return;
        }
    }

    public void OnSkill()  //���Ⱑ 1��° 
    {
        IsSkill = true;
    }

    public void MagicianSkill()
    {
        switch (magicianSlimeType)
        {
            case MagicianSlimeType.Epic:
                EpicMagicianSkill();
                break;
            case MagicianSlimeType.Legend:
                if (gameObject.name.Contains("Wizard"))
                {
                    Debug.Log("���� �⤾����");
                    LegendLizardSkill();
                }
                else
                {
                    LegendMagicianSkill();
                }
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

    public void LegendLizardSkill()
    {
        MagicArrow(target, skillPrefab);
        StopNavAgent();
        StartCoroutine(ResumeMovementAfterAttack());
        StartCoroutine(ActivateWeaponCollider());
    }
    public void LegendMagicianSkill()
    {
        Instantiate(skillPrefab, transform.position, transform.rotation);
        StopNavAgent();
        StartCoroutine(ResumeMovementAfterAttack());
        StartCoroutine(ActivateWeaponCollider());
    }


}
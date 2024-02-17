using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class FlyingDemon : MonoBehaviour, IEnemy
{
    //������Ʈ��
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

    [field: SerializeField]
    public float DropJellyPower { get; set; }

    [Header("Addictional Data")]
    private float nextAttackTime; //�����ֱ� ���� �ʱ�ȭ��
    private float detectionRadius = 20f; //�� ���� �ݰ�
    private float detectionInterval = 0.5f;  // ���� Ž�� �ֱ�
    private float sinceLastDetectionTime = 0f; // Ž�� �ֱ� �ʱ�ȭ��


    [Header("Weapon")]
    public Collider weaponCollider;
    public EnemyWeapon enemyWeapon;
    public bool isFire = false;

    [Header("Stun")]
    private bool isStunned = false; // ���� ���� ���� ����

    [Header("BossMonster")]
    // Barrier
    public GameObject barrierPrefab;
    private bool barrierActivated = false;
    private GameObject activeBarrier = null; // Ȱ��ȭ�� �� ������Ʈ
    private float originalDefense; // ���� ���� ����

    // Buff
    public GameObject buffPrefab;
    public bool buffActivated = false;

    // Bress
    public bool IsSkill { get; set; }
    private float lastSkillActivationTime = 0f;
    private float skillActivationInterval = 10f; // ��ų�� Ȱ��ȭ�� ���� (10��)
    public Transform firePoint;
    public float magicSpeed = 20f;
    public GameObject magicPrefab;



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
        }
        else
        {
            Debug.LogError("Enemy data not found for " + enemyPrefabName);
        }

        slimeCastle = GameObject.FindWithTag("SlimeCastle").transform;
    }

    private void Start()
    {
        //���ӿ�����Ʈ �� ���� �� �±׸� ���� ������Ʈ�� Ʈ�������� ���� ������ ��
        CurrentHP = MaxHP;
        if (slimeCastle != null)
        {
            target = slimeCastle.transform;  //Ÿ�ٿ� �ֱ�
            MoveToTarget(target);// ���� �� ���� ������ �̵�
        }
        else
        {
            Debug.LogError("SlimeCastle not found in the scene.");
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

        enemyWeapon.weaponDamage = AttackDamage;

        originalDefense = Defense;
    }
    void Update()
    {
        if (isDead || isStunned) return; // �׾��ų� ���� �����̸� �Ʒ� ���� ���� �� ��

        sinceLastDetectionTime += Time.deltaTime; //�ð��帧 �������� ����ȭ
        if (sinceLastDetectionTime >= detectionInterval) //������ĵ ���ݺ��� �ð� �帧�� ũ��
        {
            SearchSlimeInDetection(); //���� ��ĵ
            sinceLastDetectionTime = 0f; //�ð� �ʱ�ȭ
        }

        if (Time.time - lastSkillActivationTime >= skillActivationInterval)
        {
            IsSkill = true;
            lastSkillActivationTime = Time.time; // ������ ��ų Ȱ��ȭ �ð� ������Ʈ
        }

        // ü���� 60% ���Ϸ� ��������, ���� ���� Ȱ��ȭ���� �ʾҴٸ�
        if (CurrentHP / MaxHP <= 0.6f && !barrierActivated)
        {
            ActivateBarrier(); // �� Ȱ��ȭ �Լ� ȣ��
        }
        if (CurrentHP / MaxHP <= 0.3f && !buffActivated)
        {
            RemoveBarrierAndBuffUp(); // �� ���� �� ���ݷ� ���� �Լ� ȣ��
        }


        if (target != null) //Ÿ���� ������
        {
            MoveToTarget(target); //Ÿ�������� �׺�޽� �̵�

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
                        FlyingDemonSkill();
                    }
                    else
                    {
                        Attack(); //����, �ִϸ��̼��� �ֱ������� ������ �ϱ� ����
                    }

                    nextAttackTime = Time.time + AttackSpeed; //���� ��Ÿ�� ���� �ʱ�ȭ��
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
        Transform closestSlime = FindClosestSlime(hitColliders); //����� ���� ��ġ ����

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
        Transform closestEnemy = null; //���� ����� ���� ��ġ
        float closestDistance = Mathf.Infinity; //���� ��������� �Ÿ�

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Slime")) //�� �±׸�
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

    void Attack()//����
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
        yield return new WaitForSeconds(0.2f);
        weaponCollider.enabled = true; // weaponCollider�� Ȱ��ȭ
        yield return new WaitForSeconds(0.5f); // 0.5�� ���
        weaponCollider.enabled = false; // weaponCollider�� �ٽ� ��Ȱ��ȭ
    }

    public void GetHit(float damage) //�������� ����
    {
        // ���� ����� ���: ���ݷ� - (���� * 0.5)
        float actualDamage = damage - (Defense * 0.5f);
        // ���� ������� 0���� ������, 0���� ó���Ͽ� �������� ���� ��
        actualDamage = Mathf.Max(actualDamage, 0);
        CurrentHP -= actualDamage; //���� ����������ŭ ����

        Debug.Log("Enemy : " + CurrentHP);

        if (CurrentHP <= 0)
        {
            isDead = true;
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
        SlimeSpawnManager.instance.jellyPower += DropJellyPower;
        Destroy(gameObject); //������Ʈ ����
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SlimeWeapon"))
        {
            Debug.Log("SlimeWeapon");
            Debug.Log(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
            GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);

        }
        else if (other.gameObject.CompareTag("SlimeProjectileWeapon"))
        {
            SlimeWeapon slimeWeapon = other.gameObject.GetComponent<SlimeWeapon>();
            if (slimeWeapon != null)
            {
                GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
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
        if (!isStunned) // �̹� ���� ���°� �ƴ϶��
        {
            StartCoroutine(StunDuration(duration));
        }
    }

    IEnumerator StunDuration(float duration)
    {
        isStunned = true; // ���� ���·� ��ȯ
        navAgent.isStopped = true; // ���� �̵� ����

        yield return new WaitForSeconds(duration); // ���� ���� �ð� ���

        if (!isDead) // ���� ���� ��, ���Ͱ� ����ִٸ�
        {
            isStunned = false; // ���� ���� ����
            navAgent.isStopped = false; // ���� �̵� �簳
        }
    }

    public void FlyingDemonSkill()
    {
        anim.SetTrigger("Attack03");
        MagicArrow(target, magicPrefab);
        StopNavAgent();
        StartCoroutine(ResumeMovementAfterAttack());
    }

    void MagicArrow(Transform target, GameObject magicPrefab)
    {
        // Ÿ���� ��ġ�� ���� ȭ���� ��ȯ�մϴ�.
        GameObject arrow = Instantiate(magicPrefab, target.position, Quaternion.identity);

        // ȭ���� ��ų ���ط��� �����մϴ�.
        arrow.GetComponent<FlyingDemonSkill>().explosionArea.GetComponent<EnemyWeapon>().weaponDamage = AttackDamage * 0.7f;
    }

    void ActivateBarrier()
    {
        if (barrierPrefab != null) // �� �������� �����Ǿ� �ִٸ�
        {
            activeBarrier = Instantiate(barrierPrefab, transform.position, Quaternion.identity, transform); // ���� ��ġ�� �� ����
            Defense += Defense * 0.15f; // ���� 15% ����
            barrierActivated = true; // �� Ȱ��ȭ ���¸� true�� ����
        }
    }

    void RemoveBarrierAndBuffUp()
    {
        buffActivated = true;

        if (activeBarrier != null)
        {
            Destroy(activeBarrier); // Ȱ��ȭ�� �� ����
            activeBarrier = null; // ���� �ʱ�ȭ
        }
        Instantiate(buffPrefab, transform.position, Quaternion.identity, transform); // ���� ��ġ�� ���� ����
        Defense = originalDefense; // ������ ������� ����
        AttackDamage += AttackDamage * 0.15f; // ���ݷ��� ���� ���ݷ��� 15% ����
        barrierActivated = false; // �� ��Ȱ��ȭ ���·� ����
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EvilMage : MonoBehaviour, IEnemy
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


    [Header("Magician")]
    public GameObject magicPrefab;
    public float magicSpeed = 20f;
    public Transform firePoint;
    public bool isFire = false;
    public bool IsSkill { get; set; }

    [Header("Stun")]
    private bool isStunned = false; // ���� ���� ���� ����

    [Header("BossMonster")]
    private bool hasSpawnedAllies = false; // �Ʊ� ���� ��ȯ ���� üũ
    private bool hasLockedJelly = false; // ��ȯ �ڽ�Ʈ ��� ���� üũ

    public bool isNormalBoss = true;

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

        magicPrefab.GetComponent<EnemyWeapon>().weaponDamage = AttackDamage;
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

        if (CurrentHP / MaxHP <= 0.7f && !hasSpawnedAllies)
        {
            SpawnEnemySkill();
            hasSpawnedAllies = true; // �Ʊ� ���͸� ��ȯ������ ǥ��
        }

        if (CurrentHP / MaxHP <= 0.30f && !hasLockedJelly && !isNormalBoss)
        {
            LockJellySkill();
            hasLockedJelly = true; // ��ȯ �ڽ�Ʈ�� �ᰬ���� ǥ��
        }

        // ü���� 15% �����̰� ���� ��ȯ �ڽ�Ʈ�� ����� �ʾҴٸ� ��� ��ų ����
        if (CurrentHP / MaxHP <= 0.15f && !hasLockedJelly && isNormalBoss)
        {
            LockJellySkill();
            hasLockedJelly = true; // ��ȯ �ڽ�Ʈ�� �ᰬ���� ǥ��
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
                    Attack(); //����, �ִϸ��̼��� �ֱ������� ������ �ϱ� ����
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
        StartCoroutine(MagicArrow(target, magicPrefab));

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
        yield return new WaitForSeconds(0.5f); // 0.5�� ���
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

    void SpawnEnemySkill()
    {
        // ��ȯ�� ������ �ε����� �����մϴ�.
        int[] spawnIndexes = new int[] { 4, 5, 5, 6 };

        if (!isNormalBoss)
        {
            spawnIndexes = new int[] { 19, 20, 21, 22, 23 };
        }

        // �� �ε����� ���� ���͸� ��ȯ�մϴ�.
        for (int i = 0; i < spawnIndexes.Length; i++)
        {
            // ���� �ε����� �ش��ϴ� ���͸� ��ȯ�մϴ�.
            GameObject spawnedEnemy = Instantiate(
                EnemySpawnManager.instance.enemyPrefab[spawnIndexes[i]],
                transform.position,
                Quaternion.identity);

            // ���Ͱ� ��ȭ �����̰�, ��ȭ ����� ������ �ƴ� ��� ��ȭ ó���� �����մϴ�.
            if (EnemySpawnManager.instance.isEnhanced && EnhanceObject.Instance.objectType != ObjectType.Jelly)
            {
                EnhanceObject.Instance.EnhancedEnemy(spawnedEnemy);
            }
        }
    }

    void LockJellySkill()
    {
        // ��ȯ �ڽ�Ʈ ��� ���� ����
        Debug.Log("Locking jelly spawn cost!");
        SlimeSpawnManager.instance.lockJelly = true;
        UIManager.instance.lockImage.SetActive(true);
    }

    // MagicArrow �Լ��� �ڷ�ƾ���� ����
    IEnumerator MagicArrow(Transform target, GameObject magicPrefab)
    {
        for (int i = 0; i < 3; i++) // 3���� ������ �ݺ�
        {
            if (target == null) continue;
            GameObject arrow = Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
            arrow.transform.LookAt(target.position);

            Rigidbody arrowRigid = arrow.GetComponent<Rigidbody>();
            if (arrowRigid != null)
            {
                Vector3 direction = (target.position - firePoint.position).normalized;
                arrowRigid.velocity = direction * magicSpeed;
            }

            yield return new WaitForSeconds(0.1f); // ���� ���ݱ��� 0.1�� ���
        }
    }
}

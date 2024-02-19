using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BalanceSlimeBehaviour : MonoBehaviour, ISlime
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
    public Collider weaponCollider;
    public SlimeWeapon slimeWeapon;
    public bool isFire;
    public bool IsSkill { get; set; }

    public ClassType classType;

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
            navAgent.speed = slimeData.MoveSpeed;
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
        slimeWeapon.weaponDamage = AttackDamage;
        slimeWeapon.classType = classType;
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

    public void SlimeWeaponDamageUpdate()
    {
        slimeWeapon.weaponDamage = AttackDamage;
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
   
    void Attack()//����
    {
        anim.SetTrigger("Attack03"); 
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
        yield return new WaitForSeconds(0.5f);
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

        Debug.Log("Slime HP : " + CurrentHP); 

        if (CurrentHP <= 0)
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
        if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("EnemyCastle"))
        {
            weaponCollider.enabled = false;
        }
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

    void ISlime.OnSkill()
    {
        throw new System.NotImplementedException();
    }
}
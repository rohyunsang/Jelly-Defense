using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    public GameObject playerPrefab; //�Ʊ� ���� ������
    public Transform spawnPoint; //�Ʊ� ���� ���� ��� 
   
    public float jellyPower = 0; //�Ʊ� ���� ��ȯ ������ �ڽ�Ʈ
    private NavMeshAgent slimeAgent; 
    public Transform enemyCastle;
    public float detectionRadius = 1f; //�Ʊ� ������ �� ���� �ݰ�
    Transform closestEnemy;

    public int slimeHP = 10; //�Ʊ� ���� ü��
    public float damageCooldown = 1f; // 1�ʿ� �� ������ ü���� �����ϵ��� ����
    private float nextDamageTime;
    bool isDead = false;

    private Animator animator; 

   // private float timeUpdate = 0.0f; //�ð� ������Ʈ

    void Awake()
    {
        slimeAgent = playerPrefab.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        slimeAgent.SetDestination(enemyCastle.position);
    }
    void Update()
    {

        if (slimeHP <= 0)
        {
            StopSlimeAgent();
            slimeAgent.enabled = false;
            Die();
        }

        jellyPower += Time.deltaTime; //�ð� ������ ������ ����

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Debug.Log("Enemy");
                float enemyDistance = Vector3.Distance(transform.position, col.transform.position);

                if (enemyDistance < closestDistance)
                {
                    closestDistance = enemyDistance;
                       closestEnemy = col.transform;
                }
            }
        }
        if (closestEnemy != null)
        {
            // ���� ����� ���� ���� �̵�
            slimeAgent.SetDestination(closestEnemy.position);
            animator.SetTrigger("Attack01");
        }
        else
        {
            // ���� ����� ���� ������ ������ ���� ���� �̵�
            slimeAgent.SetDestination(enemyCastle.position);
            animator.SetBool("isMove",true);
        }
    }

    void StopSlimeAgent()
    {
        // NavMeshAgent�� ���߱�
        if (slimeAgent != null)
        {
            slimeAgent.isStopped = true;
        }
    }
    public void PlayerSpawn() // Canvas - Spawn Button 
    {
        slimeHP = 10;
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        jellyPower -= 1f;
        Debug.Log(jellyPower);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && Time.time >= nextDamageTime)
        {
            GetHit(1); // HP�� 1 ����
            nextDamageTime = Time.time + damageCooldown; // ���� ������ �ð� ����
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Time.time >= nextDamageTime)
        {
            GetHit(1); // HP�� 1 ����
            nextDamageTime = Time.time + damageCooldown; // ���� ������ �ð� ����
            if (isDead)
            {
            }
        }
    }
    void GetHit(int damage)
    {
        slimeHP -= damage;
        Debug.Log(slimeHP);
        if (slimeHP <= 0)
        {
            Invoke("Die",5);
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");
        slimeAgent.enabled = true;
        Destroy(gameObject);
    }

}
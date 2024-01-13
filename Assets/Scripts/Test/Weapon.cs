using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float speed;
    public CapsuleCollider capsuleCollider;
    public TrailRenderer trailEffect;
    private NavMeshAgent navAgent;

    bool fDown;
    bool isDead = false;
    bool isFireReady;
    float fireDelay;


    private Animator animator;

    public float detectionRadius = 10f; //�Ʊ� ������ �� ���� �ݰ�
    public float damageInterval = 1f; // �������� ���� �ֱ�
    private float nextDamageTime; //���� �������� ���� Ÿ�̹�

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Attacks");
            StartCoroutine("Attacks");
        }
    }

    void Attack()
    {
        if (navAgent.isStopped) //�׺�����Ʈ�� ��������
            return;
        fireDelay += Time.deltaTime; //���� �����̿� �ð� ���ϱ�
        isFireReady = damageInterval < fireDelay;

        if(isFireReady && !isDead)
        {
            Use();
            animator.SetTrigger("Attack01");
            fireDelay -= damageInterval;
        }
    }

    IEnumerator Attacks()
    {
        //1
        yield return new WaitForSeconds(0.1f); //������ ���
        //�ݸ��� Ȱ��ȭ
        //����Ʈ Ȱ��ȭ
        yield return new WaitForSeconds(0.3f); 
        //�ݸ��� ��Ȱ��ȭ

        yield return new WaitForSeconds(0.3f); 
        //����Ʈ ��Ȱ��ȭ

        //yield return null; //1������ ���

    }
}

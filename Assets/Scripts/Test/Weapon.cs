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

    public float detectionRadius = 10f; //아군 유닛의 적 감지 반경
    public float damageInterval = 1f; // 데미지를 받을 주기
    private float nextDamageTime; //다음 데미지를 받을 타이밍

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
        if (navAgent.isStopped) //네비에이전트가 멈췄으면
            return;
        fireDelay += Time.deltaTime; //공격 딜레이에 시간 더하기
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
        yield return new WaitForSeconds(0.1f); //프레임 대기
        //콜리더 활성화
        //이펙트 활성화
        yield return new WaitForSeconds(0.3f); 
        //콜리더 비활성화

        yield return new WaitForSeconds(0.3f); 
        //이펙트 비활성화

        //yield return null; //1프레임 대기

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    public GameObject prefabPlayer; //아군 유닛 프리팹
    public Transform pointSpawn; //아군 유닛 스폰 장소
    public float JellyPower = 0; //아군 유닛 소환 젤리력 코스트
    private NavMeshAgent slimeAgent;
    public Transform enemyCastle;
    public float detectionRadius = 10f; //아군 유닛의 적 감지 반경


    private float timeUpdate = 0.0f; //시간 업데이트

    void Start()
    {
        slimeAgent = GetComponent<NavMeshAgent>();
        slimeAgent.SetDestination(enemyCastle.position);
    }

    // Update is called once per frame
    void Update()
    {
        JellyPower += Time.deltaTime; //시간 증가시 젤리력 증가


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = col.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            // 가장 가까운 적을 향해 이동
            slimeAgent.SetDestination(closestEnemy.position);
        }
}

    public void PlayerSpawn()
    {
        Instantiate(prefabPlayer, pointSpawn.position, pointSpawn.rotation);
        JellyPower -= 1f;
       //Debug.Log();
    }


}

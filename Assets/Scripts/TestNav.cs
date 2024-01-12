using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNav : MonoBehaviour
{
    private NavMeshAgent slimeAgent;
    public Transform enemyCastle;
    public float detectionRadius = 500f; // 아군 유닛의 적 감지 반경

    void Start()
    {
        slimeAgent = GetComponent<NavMeshAgent>();
        slimeAgent.SetDestination(enemyCastle.position);
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("enemy")) // 적군 태그면 
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
        else
        {
            // 가장 가까운 적이 없으면 적군의 성을 향해 이동
            slimeAgent.SetDestination(enemyCastle.position);
        }
    }
}

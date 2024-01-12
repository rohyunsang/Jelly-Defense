using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNav : MonoBehaviour
{
    private NavMeshAgent slimeAgent;
    public Transform enemyCastle;
    public float detectionRadius = 500f; // �Ʊ� ������ �� ���� �ݰ�

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
            if (col.CompareTag("enemy")) // ���� �±׸� 
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
            // ���� ����� ���� ���� �̵�
            slimeAgent.SetDestination(closestEnemy.position);
        }
        else
        {
            // ���� ����� ���� ������ ������ ���� ���� �̵�
            slimeAgent.SetDestination(enemyCastle.position);
        }
    }
}

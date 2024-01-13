using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNav : MonoBehaviour
{
    private NavMeshAgent slimeAgent;
    public Transform enemyCastle;
    public float detectionRadius = 10f; // �Ʊ� ������ �� ���� �ݰ�

    void Start()
    {
        slimeAgent = GetComponent<NavMeshAgent>();
        slimeAgent.SetDestination(enemyCastle.position);
    }

    // Update is called once per frame
    void Update()
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy")) // ���� �±׸� 
            {
                Debug.Log("Enemy");
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

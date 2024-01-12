using UnityEngine;
using UnityEngine.AI;

public class SlimeAgentController : MonoBehaviour
{
    public float detectionRadius = 10f;
    private NavMeshAgent slimeAgent;

    void Start()
    {
        slimeAgent = GetComponent<NavMeshAgent>();

        // NavMeshAgent�� �ʱ�ȭ�� �� NavMesh�� ����Ǿ� �־�� �մϴ�.
        if (slimeAgent.isOnNavMesh)
        {
            // NavMeshAgent�� Ȱ��ȭ�ϰ� �ʱ� ��ġ�� �̵�
            slimeAgent.enabled = true;
            slimeAgent.SetDestination(transform.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent is not on NavMesh!");
        }
    }

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        Transform closestEnemy = FindClosestEnemy(hitColliders);

        if (closestEnemy != null)
        {
            // ���� ����� ���� ���� �̵�
            slimeAgent.SetDestination(closestEnemy.position);
        }
    }

    Transform FindClosestEnemy(Collider[] colliders)
    {
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = col.transform;
                }
            }
        }

        return closestEnemy;
    }
}

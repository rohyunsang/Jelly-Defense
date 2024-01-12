using UnityEngine;
using UnityEngine.AI;

public class SlimeAgentController : MonoBehaviour
{
    public float detectionRadius = 10f;
    private NavMeshAgent slimeAgent;

    void Start()
    {
        slimeAgent = GetComponent<NavMeshAgent>();

        // NavMeshAgent를 초기화할 때 NavMesh에 적용되어 있어야 합니다.
        if (slimeAgent.isOnNavMesh)
        {
            // NavMeshAgent를 활성화하고 초기 위치로 이동
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
            // 가장 가까운 적을 향해 이동
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

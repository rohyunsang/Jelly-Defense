using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    public GameObject prefabPlayer; //�Ʊ� ���� ������
    public Transform pointSpawn; //�Ʊ� ���� ���� ���
    public float JellyPower = 0; //�Ʊ� ���� ��ȯ ������ �ڽ�Ʈ
    private NavMeshAgent slimeAgent;
    public Transform enemyCastle;
    public float detectionRadius = 10f; //�Ʊ� ������ �� ���� �ݰ�


    private float timeUpdate = 0.0f; //�ð� ������Ʈ

    void Start()
    {
        slimeAgent = GetComponent<NavMeshAgent>();
        slimeAgent.SetDestination(enemyCastle.position);
    }

    // Update is called once per frame
    void Update()
    {
        JellyPower += Time.deltaTime; //�ð� ������ ������ ����


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
            // ���� ����� ���� ���� �̵�
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

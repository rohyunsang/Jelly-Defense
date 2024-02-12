using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArrow : MonoBehaviour
{
    public GameObject bombArrowEffect;
    private Rigidbody rb; // Rigidbody ������Ʈ ����
    public GameObject explosionArea;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
        Destroy(gameObject, 5f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyCastle"))
        {
            bombArrowEffect.SetActive(true);
            StopMovement(); // ȭ���� �̵��� ����
            explosionArea.SetActive(true); // ���� ���� Ȱ��ȭ
            Destroy(gameObject, 1f);
        }
    }

    private void StopMovement()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // ȭ���� �ӵ��� 0���� ����
            rb.isKinematic = true; // ���� ������ ��Ȱ��ȭ�Ͽ� �߰����� �̵� ����
        }
    }
}

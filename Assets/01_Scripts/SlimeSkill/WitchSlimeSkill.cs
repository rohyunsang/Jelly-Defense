using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchSlimeSkill : MonoBehaviour
{
    public GameObject witchSkillEffect;
    private Rigidbody rb; // Rigidbody ������Ʈ ����
    public GameObject explosionArea;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyCastle"))
        {
            witchSkillEffect.SetActive(true);
            StopMovement(); // ������ �̵��� ����
            explosionArea.SetActive(true); // ���� ���� Ȱ��ȭ
            Destroy(gameObject, 1f);
        }
    }

    private void StopMovement()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // ������ �ӵ��� 0���� ����
            rb.isKinematic = true; // ���� ������ ��Ȱ��ȭ�Ͽ� �߰����� �̵� ����
        }
    }
}

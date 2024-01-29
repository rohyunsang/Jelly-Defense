using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSlimeSkill : MonoBehaviour
{
    public GameObject wizardSkillEffect;
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
            GetComponent<MeshRenderer>().enabled = false;
            wizardSkillEffect.SetActive(true);
            StopMovement(); // ������ �̵��� ����
            explosionArea.SetActive(true); // ���� ���� Ȱ��ȭ
            Destroy(gameObject, 2f);
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

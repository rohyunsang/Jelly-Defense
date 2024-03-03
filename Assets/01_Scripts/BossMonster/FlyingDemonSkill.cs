using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDemonSkill : MonoBehaviour
{
    public GameObject flyingDemonSkillEffect;
    private Rigidbody rb; // Rigidbody ������Ʈ ����
    public GameObject explosionArea;
    public float skillWeaponDemage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Slime") || other.gameObject.CompareTag("SlimeCastle"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            flyingDemonSkillEffect.SetActive(true);
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

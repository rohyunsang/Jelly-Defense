using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSlimeSkill : MonoBehaviour
{
    public GameObject buffEffect;
    public SphereCollider sphereCollider;
    private ISlime affectedSlime;
    private float originalAttackDamage;
    private bool isActive = true;

    void Start()
    {
        sphereCollider.enabled = true;
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_SkillSoundIncreaseAttack);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (isActive && other.gameObject.CompareTag("Slime"))
        {
            affectedSlime = other.gameObject.GetComponent<ISlime>();
            if (affectedSlime != null)
            {
                // ���� �������� ���� ���ݷ� ����
                originalAttackDamage = affectedSlime.AttackDamage;

                // ������ ���ݷ� 1.5�� ����
                affectedSlime.AttackDamage *= 1.5f;

                // ����Ʈ ����
                GameObject effect = Instantiate(buffEffect, other.transform.position, Quaternion.identity);

                // 7�� �Ŀ� ����Ʈ ���� �� �������� ���ݷ� ������� ��������
                StartCoroutine(DeactivateSkill(effect));
            }
        }
    }

    private IEnumerator DeactivateSkill(GameObject effect)
    {
        yield return new WaitForSeconds(2f);
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }


        yield return new WaitForSeconds(5f);

        // 7�� �Ŀ� ����Ʈ ����
        Destroy(effect);

        // �������� ���ݷ� ������� ��������
        if (affectedSlime != null)
        {
            affectedSlime.AttackDamage = originalAttackDamage;
        }

        // ��ų�� �ٽ� Ȱ��ȭ���� �ʵ��� ����
        isActive = false;

        // 2�� �Ŀ� ��ų ��ü�� ����
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

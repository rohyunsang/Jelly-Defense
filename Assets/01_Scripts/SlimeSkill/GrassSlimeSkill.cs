using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSlimeSkill : MonoBehaviour
{
    public GameObject healingEffect;
    public SphereCollider sphereCollider;

    void Start()
    {
        sphereCollider.enabled = true;
        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_SkillSoundHeal);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slime"))
        {
            ISlime slime = other.gameObject.GetComponent<ISlime>();
            if (slime != null)
            {
                float healAmount = slime.MaxHP * 0.5f; // 30% �� ���
                slime.CurrentHP = Mathf.Min(slime.CurrentHP + healAmount, slime.MaxHP); // ���� ü���� �ִ� ü���� �ʰ����� �ʵ��� ����

                // ����Ʈ ����
                GameObject effect = Instantiate(healingEffect, other.transform.position, Quaternion.identity);

                // 2�� �Ŀ� ����Ʈ ����
                Destroy(effect, 2f);
            }
            Destroy(gameObject, 2f);
        }
    }
}

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
                float healAmount = slime.MaxHP * 0.5f; // 30% 힐 계산
                slime.CurrentHP = Mathf.Min(slime.CurrentHP + healAmount, slime.MaxHP); // 현재 체력이 최대 체력을 초과하지 않도록 설정

                // 이펙트 생성
                GameObject effect = Instantiate(healingEffect, other.transform.position, Quaternion.identity);

                // 2초 후에 이펙트 삭제
                Destroy(effect, 2f);
            }
            Destroy(gameObject, 2f);
        }
    }
}

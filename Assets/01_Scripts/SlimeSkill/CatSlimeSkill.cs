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
                // 현재 슬라임의 원래 공격력 저장
                originalAttackDamage = affectedSlime.AttackDamage;

                // 슬라임 공격력 1.5배 증가
                affectedSlime.AttackDamage *= 1.5f;

                // 이펙트 생성
                GameObject effect = Instantiate(buffEffect, other.transform.position, Quaternion.identity);

                // 7초 후에 이펙트 삭제 및 슬라임의 공격력 원래대로 돌려놓기
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

        // 7초 후에 이펙트 삭제
        Destroy(effect);

        // 슬라임의 공격력 원래대로 돌려놓기
        if (affectedSlime != null)
        {
            affectedSlime.AttackDamage = originalAttackDamage;
        }

        // 스킬을 다시 활성화하지 않도록 설정
        isActive = false;

        // 2초 후에 스킬 자체를 삭제
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

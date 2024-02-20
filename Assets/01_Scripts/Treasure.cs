using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public float slimeTotalDamage = 0f;
    public float enemyTotalDamage = 0f;
    private Animator anim;
    public TreasureSlimeHit treasureSlimeHit;
    public TreasureEnemyHit treasureEnemyHit;




    void Start()
    {
        slimeTotalDamage = 0f;
        enemyTotalDamage = 0f;

        anim = GetComponent<Animator>();
        
        Invoke("CompareTotalDamage", 20f);
    }

    public void CompareTotalDamage()
    {
        anim.SetTrigger("Open");

        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_ObjectOpenSound);

        slimeTotalDamage = treasureSlimeHit.totalDamage;
        enemyTotalDamage += treasureEnemyHit.totalDamage;

        if(slimeTotalDamage > enemyTotalDamage)
        {
            StageManager.Instance.objectOpen = true;

            SlimeSpawnManager.instance.isEnhanced = true;

            GameObject slimeCastle = GameObject.FindWithTag("SlimeCastle");

            //Instantiate(EnhanceObject.Instance.buffEffect, slimeCastle.transform.position, Quaternion.identity, slimeCastle.transform);
            UIManager.instance.objectImage.SetActive(true);
            UIManager.instance.objectShining.SetActive(true);

            switch (EnhanceObject.Instance.objectType)
            {
                case ObjectType.Hp:
                    UIManager.instance.objectDesText.text = "ü�� + 10%";
                    break;
                case ObjectType.Attack:
                    UIManager.instance.objectDesText.text = "���ݷ� + 10%";
                    break;
                case ObjectType.Defense:
                    UIManager.instance.objectDesText.text = "���� + 10%";
                    break;
                case ObjectType.AttackSpeed:
                    UIManager.instance.objectDesText.text = "���� �ӵ� + 10%";
                    break;
                case ObjectType.Jelly:
                    UIManager.instance.objectDesText.text = "������ + 100";
                    if (SlimeSpawnManager.instance.maxJellyPower < SlimeSpawnManager.instance.jellyPower + 100f)
                        SlimeSpawnManager.instance.jellyPower = SlimeSpawnManager.instance.maxJellyPower;
                    else
                        SlimeSpawnManager.instance.jellyPower += 100f;
                    break;

            }
        }

        else if(slimeTotalDamage < enemyTotalDamage)
        {
            EnemySpawnManager.instance.isEnhanced = true;

            GameObject enemyCastle = GameObject.FindWithTag("EnemyCastle");

            // buffEffect�� slimeCastle�� ��ġ�� �ν��Ͻ�ȭ�մϴ�.
            GameObject effectInstance = Instantiate(EnhanceObject.Instance.buffEffect, enemyCastle.transform.position, Quaternion.identity, enemyCastle.transform);

            // �ν��Ͻ�ȭ�� ������Ʈ�� �������� 10��� �����մϴ�.
            effectInstance.transform.localScale = new Vector3(10, 10, 10);


            if (EnhanceObject.Instance.objectType == ObjectType.Jelly)
            {
                SlimeSpawnManager.instance.jellyPower -= 100f;
            }
        }
        else
        {
            return;
        }
        Destroy(gameObject, 1f);

    }

}

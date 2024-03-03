using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeCastle : MonoBehaviour
{
    public float HP = 1000f;
    public float currentHP;
    public TextMeshPro slimeCastleHPTMP;

    public bool isFailed = false;
    

    void Start()
    {
        currentHP = HP;
        slimeCastleHPTMP.text = currentHP.ToString("F0");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slime") || other.gameObject.CompareTag("SlimeProjectileWeapon")
            || other.gameObject.CompareTag("SlimeWeapon"))
            return;
        if (other.transform.CompareTag("EnemyWeapon"))
        {
            GetHit(other.gameObject.GetComponent<EnemyWeapon>().weaponDamage);
        }
        else if (other.transform.CompareTag("EnemyProjectileWeapon"))
        {
            EnemyWeapon enemyWeapon = other.gameObject.GetComponent<EnemyWeapon>();
            if (enemyWeapon != null)
            {
                GetHit(other.gameObject.GetComponent<EnemyWeapon>().weaponDamage);
                Destroy(other.gameObject);
            }

        }
        else
        {
            return;
        }
    }

    public void GetHit(float damage) //�������� ����
    {
        currentHP -= damage; //���� ����������ŭ ����
        Debug.Log("Slime Castle HP : " + currentHP); //�ܼ�â�� ���
        slimeCastleHPTMP.text = currentHP.ToString("F0");

        if (currentHP <= 0)
        {
            if (isFailed) return;
            isFailed = true;
            AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_StageFailSound);
            UIManager.instance.OnStageFailScreen();
        }
    }
}

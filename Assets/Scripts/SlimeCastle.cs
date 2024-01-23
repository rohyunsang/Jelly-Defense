using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeCastle : MonoBehaviour
{
    public float HP = 1000f;
    public float currentHP;
    public TextMeshPro slimeCastleHPTMP;
    

    void Start()
    {
        currentHP = HP;
        slimeCastleHPTMP.text = currentHP.ToString("F0");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("EnemyWeapon"))
        {
            GetHit(other.gameObject.GetComponent<EnemyWeapon>().weaponDamage);
        }
    }

    public void GetHit(float damage) //�������� ����
    {
        currentHP -= damage; //���� ����������ŭ ����
        Debug.Log("Slime Castle HP : " + currentHP); //�ܼ�â�� ���
        slimeCastleHPTMP.text = currentHP.ToString("F0");

        if (currentHP <= 0)
        {
            UIManager.instance.OnStageFailScreen();
        }
    }
}
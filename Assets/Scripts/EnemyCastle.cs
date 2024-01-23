using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCastle : MonoBehaviour
{
    public float HP = 1000f;
    public float currentHP;
    public TextMeshPro enemyCastleHPTMP;

    void Start() // �����Ҷ� ����ž���.
    {
        currentHP = HP;
        enemyCastleHPTMP.text = currentHP.ToString("F0");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("SlimeWeapon"))
        {
            GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
        }
    }

    public void GetHit(float damage) //�������� ����
    {
        currentHP -= damage; //���� ����������ŭ ����
        Debug.Log("Enemy Castle HP : " + currentHP); //�ܼ�â�� ���
        enemyCastleHPTMP.text = currentHP.ToString("F0");

        if (currentHP <= 0)
        {
            UIManager.instance.OnStageClearScreen();
            
        }
    }
}

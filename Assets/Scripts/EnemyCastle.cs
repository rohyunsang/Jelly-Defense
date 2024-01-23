using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCastle : MonoBehaviour
{
    public float HP = 1000f;
    public float currentHP;
    public TextMeshPro enemyCastleHPTMP;

    void Start() // 시작할때 실행돼야함.
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

    public void GetHit(float damage) //데미지를 받음
    {
        currentHP -= damage; //받을 데미지량만큼 감소
        Debug.Log("Enemy Castle HP : " + currentHP); //콘솔창에 출력
        enemyCastleHPTMP.text = currentHP.ToString("F0");

        if (currentHP <= 0)
        {
            UIManager.instance.OnStageClearScreen();
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCastle : MonoBehaviour
{
    public float HP = 3000f;
    public float currentHP;
    public TextMeshPro enemyCastleHPTMP;
    [SerializeField] bool isClear = false;

    void Start() // 시작할때 실행돼야함.
    {
        currentHP = HP;
        enemyCastleHPTMP.text = currentHP.ToString("F0");
        isClear = false;
    }

    public void CriticalHit()
    {
        currentHP *= 0.1f;
        enemyCastleHPTMP.text = currentHP.ToString("F0");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyProjectileWeapon")
            || other.gameObject.CompareTag("EnemyWeapon")) 
            return;

        if (other.transform.CompareTag("SlimeWeapon"))
        {
            GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);

        }
        else if (other.transform.CompareTag("SlimeProjectileWeapon"))
        {
            SlimeWeapon slimeWeapon = other.gameObject.GetComponent<SlimeWeapon>();
            if (slimeWeapon != null)
            {
                GetHit(other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
                Destroy(other.gameObject);
            }
            
        }
        else
        {
            return;
        }
    }

    public void GetHit(float damage) //데미지를 받음
    {
        currentHP -= damage; //받을 데미지량만큼 감소
        Debug.Log("Enemy Castle HP : " + currentHP); //콘솔창에 출력
        enemyCastleHPTMP.text = currentHP.ToString("F0");

        if (currentHP <= 0 && !isClear)
        {
            isClear = true;

            ScenarioManager.Instance.InitScenarioStageStory(UIManager.instance.selectedStageName);
            
            StageManager.Instance.EstimateStageStar();
            UIManager.instance.OnStageClearScreen();
            AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_StageClearSound);
        }
    }
}

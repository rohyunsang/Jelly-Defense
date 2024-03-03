using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSlimeHit : MonoBehaviour
{
    public float totalDamage = 0f;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyProjectileWeapon")
            || other.gameObject.CompareTag("EnemyWeapon") || other.gameObject.CompareTag("Meteor"))
            return;

        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_ObjectAttackSound);

        if (other.transform.CompareTag("SlimeWeapon"))
        {
            totalDamage += (other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);

        }
        else if (other.transform.CompareTag("SlimeProjectileWeapon"))
        {
            SlimeWeapon slimeWeapon = other.gameObject.GetComponent<SlimeWeapon>();
            if (slimeWeapon != null)
            {
                totalDamage += (other.gameObject.GetComponent<SlimeWeapon>().weaponDamage);
                Destroy(other.gameObject);
            }

        }
        else
        {
            return;
        }
    }
}

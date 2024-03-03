using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureEnemyHit : MonoBehaviour
{
    public float totalDamage = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slime") || other.gameObject.CompareTag("SlimeProjectileWeapon")
            || other.gameObject.CompareTag("SlimeWeapon") || other.gameObject.CompareTag("Meteor"))
            return;

        AudioManager.Instance.PlaySfx(AudioManager.SFX.SFX_ObjectAttackSound);

        if (other.transform.CompareTag("EnemyWeapon"))
        {
            totalDamage += (other.gameObject.GetComponent<EnemyWeapon>().weaponDamage);
        }
        else if (other.transform.CompareTag("EnemyProjectileWeapon"))
        {
            EnemyWeapon enemyWeapon = other.gameObject.GetComponent<EnemyWeapon>();
            if (enemyWeapon != null)
            {
                totalDamage += (other.gameObject.GetComponent<EnemyWeapon>().weaponDamage);
                Destroy(other.gameObject);
            }

        }
        else
        {
            return;
        }
    }

}

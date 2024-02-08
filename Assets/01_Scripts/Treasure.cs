using System.Collections;
using System.Collections.Generic;
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
        
        Invoke("CompareTotalDamage", 10f);
    }

    public void CompareTotalDamage()
    {
        anim.SetTrigger("Open");

        slimeTotalDamage = treasureSlimeHit.totalDamage;
        enemyTotalDamage += treasureEnemyHit.totalDamage;

        if(slimeTotalDamage > enemyTotalDamage)
        {
            SlimeSpawnManager.instance.isEnhanced = true;
            if (EnhanceObject.Instance.objectType == ObjectType.Jelly)
            {
                if (SlimeSpawnManager.instance.maxJellyPower < SlimeSpawnManager.instance.jellyPower + 100f)
                    SlimeSpawnManager.instance.jellyPower = SlimeSpawnManager.instance.maxJellyPower;
                else
                    SlimeSpawnManager.instance.jellyPower += 100f;
            }
        }
        else if(slimeTotalDamage < enemyTotalDamage)
        {
            EnemySpawnManager.instance.isEnhanced = true;
            if (EnhanceObject.Instance.objectType == ObjectType.Jelly)
            {
                if (0f > SlimeSpawnManager.instance.jellyPower - 100f)
                    SlimeSpawnManager.instance.jellyPower = 0f;
                else
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{

    float AttackDamage { get; set; }
    float MaxHP { get; set; }
    float CurrentHP { get; set; }
    float Defense { get; set; }
    float AttackSpeed { get; set; }

    float DropJellyPower { get; set; }

    void GetHit(float damage);
    void GetStunned(float duration);
}

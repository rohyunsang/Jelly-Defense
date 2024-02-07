using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    
    float MaxHP { get; set; }
    float CurrentHP { get; set; }
    float Defense { get; set; }

    void GetHit(float damage);
    void GetStunned(float duration);
}

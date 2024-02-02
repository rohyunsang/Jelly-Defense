using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISlime
{
    float MaxHP { get; set; }
    float CurrentHP { get; set; }
    float AttackDamage { get; set; }
    float Defense { get; set; }
    float AttackSpeed { get; set; }
    float MoveSpeed { get; set; }
    float AttackRange { get; set; }
    
}
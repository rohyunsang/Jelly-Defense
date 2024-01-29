using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISlime
{
    float MaxHP { get; set; }
    float CurrentHP { get; set; }
    float AttackDamage { get; set; }
    // 여기에 추가로 필요한 메서드나 프로퍼티를 선언할 수 있습니다.
}
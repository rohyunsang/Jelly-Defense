using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISlime
{
    float MaxHP { get; set; }
    float CurrentHP { get; set; }
    float AttackDamage { get; set; }
    // ���⿡ �߰��� �ʿ��� �޼��峪 ������Ƽ�� ������ �� �ֽ��ϴ�.
}
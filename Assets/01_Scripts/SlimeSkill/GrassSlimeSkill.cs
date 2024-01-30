using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSlimeSkill : MonoBehaviour
{
    public GameObject healingEffect;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slime"))
        {
            ISlime slime = other.gameObject.GetComponent<ISlime>();
            if (slime != null)
            {
                float healAmount = slime.MaxHP * 0.3f; // 30% �� ���
                slime.CurrentHP = Mathf.Min(slime.CurrentHP + healAmount, slime.MaxHP); // ���� ü���� �ִ� ü���� �ʰ����� �ʵ��� ����
            }
        }
    }
}

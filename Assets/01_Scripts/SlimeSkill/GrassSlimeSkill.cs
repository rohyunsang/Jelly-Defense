using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSlimeSkill : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slime"))
        {
            
        }
    }
}

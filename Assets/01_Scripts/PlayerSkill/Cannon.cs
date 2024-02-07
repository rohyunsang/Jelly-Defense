using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public SphereCollider sphereCollider;
    void Start()
    {
        sphereCollider.enabled = true;
        Destroy(gameObject, 2f);
    }

}

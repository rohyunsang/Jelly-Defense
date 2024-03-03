using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public SphereCollider sphereCollider;
    void Start()
    {
        sphereCollider.enabled = true;
        Destroy(gameObject, 3f); 
    }

}

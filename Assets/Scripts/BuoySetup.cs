using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoySetup : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        Physics.IgnoreLayerCollision(7, 8);
        rb = gameObject.GetComponent<Rigidbody>();
        //Debug.Log("centerOfMass: " + rb.centerOfMass); // default is (0.00, 0.08, 0.00)
        rb.centerOfMass = new Vector3(0, -1, 0);
    }
}

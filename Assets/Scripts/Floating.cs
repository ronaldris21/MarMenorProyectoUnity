// Source: https://www.youtube.com/watch?v=iasDPyC0QOg

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Floating : MonoBehaviour
{
    [SerializeField] Transform[] floaters;
    [SerializeField] float underWaterDrag = 3f;
    [SerializeField] float underWaterAngularDrag = 1f;
    [SerializeField] float airDrag = 0f;
    [SerializeField] float airAngularDrag = 0.05f;
    [SerializeField] float floatingPower = 15f;
    [SerializeField] float waterHeight = 0f;

    Rigidbody Rb;
    bool Underwater;
    int floatersUnderWater;

    void Start()
    {
        Rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        floatersUnderWater = 0;
        for(int i = 0; i < floaters.Length; i++)
        {
            float diff = floaters[i].position.y - waterHeight;
            if (diff < 0)
            {
                Rb.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(diff), floaters[i].position, ForceMode.Force);
                floatersUnderWater += 1;
                if (!Underwater)
                {
                    Underwater = true;
                    SwitchState(true);
                }
            }
        }

        if (Underwater && floatersUnderWater==0)
        {
            Underwater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            Rb.drag = underWaterDrag;
            Rb.angularDrag = underWaterAngularDrag;
        }
        else
        {
            Rb.drag = airDrag;
            Rb.angularDrag = airAngularDrag;
        }
    }
}
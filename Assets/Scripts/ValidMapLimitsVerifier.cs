using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class ValidMapLimitsVerifier : MonoBehaviour
{
    //GameObject from Level/ValidMapLimit/ 4 references cubes
    public GameObject mapLimitCube1;
    public GameObject mapLimitCube2;
    public GameObject mapLimitCube3;
    public GameObject mapLimitCube4;


    public bool PointInMap(Vector3 point)
    {
        List<Vector3> mapLimits = new List<Vector3>();
        mapLimits.Add(mapLimitCube1.transform.position);
        mapLimits.Add(mapLimitCube2.transform.position);
        mapLimits.Add(mapLimitCube3.transform.position);
        mapLimits.Add(mapLimitCube4.transform.position);

        return GeometryUtils.PointInPolygon(point, mapLimits);
    }


    //Eliminar 
    public GameObject cuboPrueba;
    private void Update()
    {
        if (cuboPrueba == null)
            return;


        /*if (PointInMap(cuboPrueba.transform.position))
        {
            Debug.Log("DENTRO");
        }
        else
        {
            Debug.Log("FUERA");
        }*/
    }
}

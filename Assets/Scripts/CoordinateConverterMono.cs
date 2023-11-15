// Mismo script que el de Asset/Code/Utils/CoordinateConverter.cs de Ronald
// pero dentro de la sueprclase MonoBehaviour para poder usarlo con GameObjects

using System;
using UnityEngine;
using Assets.Code.Utils;

public class CoordinateConverterMono : MonoBehaviour
{
    public GameObject coordinateConverterPosA;
    public GameObject coordinateConverterPosB;
    public GameObject coordinateConverterPosC;
    private Vector3 q1, q2, q3;

    public float defaultAltitude = 1;

    public CoordinateConverter getCoordinateConverter()
    {

        ///Inicio el conversor de unidades de manera dinamico y elimino los objetos guía
        Transform posA = coordinateConverterPosA.transform;
        Transform posB = coordinateConverterPosB.transform;
        Transform posC = coordinateConverterPosC.transform;
        // Puntos de referencia en el nuevo sistema
        q1 = new Vector3(posA.position.x, posA.position.y, posA.position.z);
        q2 = new Vector3(posB.position.x, posB.position.y, posB.position.z);
        q3 = new Vector3(posC.position.x, posC.position.y, posC.position.z);

        return new CoordinateConverter(q1, q2, q3, defaultAltitude);
    }

}

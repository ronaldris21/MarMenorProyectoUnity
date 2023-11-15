using Assets;
using Assets.Code.Utils;
using DLL_Models_Petitions.Models;
using DLL_Models_Petitions.Models.RespuestasPeticiones;
using SmartLagoonApiCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MouseClickedScript : MonoBehaviour
{
    [SerializeField] ActionBasedController rightController; // Referencia al controlador de mano derecha
    public float raycastDistance = 100f; // Distancia del raycast

    public GameObject PanelPadreDatosSensor;
    public GameObject PanelDatoIndividual;


    void Start()
    {
    }

    async void Update()
    {

        RaycastHit hit;


        ///Raycasthit with  Mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            if (Physics.Raycast(ray, out hit,raycastDistance))
            {
                if (hit.transform)
                {
                    HitSuccess(hit);
                    return;
                }
            }

        ///RayCastHit with META QUEST
        rightController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out RaycastHit rightRaycastHit);
        Vector3 raycasHitPoint = rightRaycastHit.point;
        if((raycasHitPoint.x != 0f) || (raycasHitPoint.y != 0f) || (raycasHitPoint.z != 0f))
        {
            HitSuccess(rightRaycastHit);
            return;
        }
    }


    void HitSuccess(RaycastHit hit)
    {
        //Code When RayCastHit Successful

        #region Mostrar Latest Values

        try
        {
            Debug.Log(hit.transform.gameObject.GetComponent<SensorTags>().locationId);
            if (hit.transform.gameObject.GetComponent<SensorTags>() == null)
                return;

            String idLocation = hit.transform.gameObject.GetComponent<SensorTags>().locationId;
            String sensorTypeId = Singleton.Instance.sensorsIdLocationToDeviceTypeDictionary[idLocation];
            var sensores = Singleton.Instance.sensoresDataTypes_byLocationSensorType[sensorTypeId];
            string sensoresParametros = String.Join(",", sensores.Select(s => s.name).ToArray());
            Debug.Log(sensores.Length + " sensores: {" + sensoresParametros + "}");


            var datosUltimos = Singleton.Instance.sensorsLatestDataDictionary[sensorTypeId].data
                                        .Where(c => c.location_id == idLocation)
                                        .Select(c => new {
                                            c.sensor_type,
                                            c.value,
                                        }).ToArray();


            SensorType[] descripcionTiposSensores = Singleton.Instance.sensoresDataTypes_byLocationSensorType[sensorTypeId];
            Dictionary<string, SensorType> dictionaryTipoSensor_a_Unidad = new Dictionary<string, SensorType>();
            foreach (var item in descripcionTiposSensores)
            {
                dictionaryTipoSensor_a_Unidad.Add(item.name, item);
            }


            // Recorre todos los hijos del objeto padre y los destruye
            List<GameObject> childrenToDestroy = new List<GameObject>();
            foreach (Transform child in PanelPadreDatosSensor.transform)
            {
                childrenToDestroy.Add(child.gameObject);
            }

            


            foreach (var item in datosUltimos)
            {

                String datoSensor = item.sensor_type + ": " + item.value + " " + dictionaryTipoSensor_a_Unidad[item.sensor_type].units;
                
                Debug.Log(datoSensor);

                GameObject datoPanelito = Instantiate(PanelDatoIndividual);
                datoPanelito.transform.SetParent(PanelPadreDatosSensor.transform);

                TextMeshProUGUI textHijo = datoPanelito.GetComponentInChildren<TextMeshProUGUI>();

                textHijo.text = datoSensor;

                 



            }
            //Destruyo los hijos que no me interesan
            foreach (GameObject child in childrenToDestroy)
            {
                DestroyImmediate(child);
            }


        }
        catch (Exception e) {
            ///Por el momento da excepci�n cuando da click a algo que no es un sensor y ha entrado en el hit
            Debug.LogError(e.ToString());
        }
        #endregion


        

        #region Mostrar datos en un periodo de tiempo

        try
        {
            Debug.Log(hit.transform.gameObject.GetComponent<SensorTags>().locationId);

            String idLocation = hit.transform.gameObject.GetComponent<SensorTags>().locationId;
            String sensorTypeId = Singleton.Instance.sensorsIdLocationToDeviceTypeDictionary[idLocation];
            var sensores = Singleton.Instance.sensoresDataTypes_byLocationSensorType[sensorTypeId];
            string sensoresParametros = String.Join(",", sensores.Select(s => s.name).ToArray());
            Debug.Log(sensores.Length + " sensores: {" + sensoresParametros + "}");

            this.GetComponent<RestClient>().getSensorDataUsingDates(idLocation, sensorTypeId, sensoresParametros);


        }
        catch (Exception) { }
        #endregion

    }

}
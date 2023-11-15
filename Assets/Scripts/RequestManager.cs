using Assets;
using SmartLagoonApiCalls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;
using Assets.Code.Utils;
using System.Net.Http;
using static UnityEditor.PlayerSettings;
using UnityEditor.VersionControl;

public class RequestManager : MonoBehaviour
{

    public Material materialMetereologico;
    public Material materialCamara;
    public Material materialSatellite;
    public Material materialAEMET;
    public Material materialBoya;
    public Material materialSIAM_IMIDA;
    Dictionary<String, Material> materialsDictionary = new Dictionary<string, Material>();



    // Start is called before the first frame update
    GameObject[] sensoresUserHard = new GameObject[0];
    GameObject[] sensoresHard = new GameObject[0];
    public GameObject SensorSphere;

    Dictionary<String, GameObject[]> sensoresDictionary = new Dictionary<string, GameObject[]>();


    public GameObject toggleDatosMetereologicos;
    public GameObject toggleCamaras;
    public GameObject toggleSatellite;
    public GameObject toggleAEMET;
    public GameObject toggleBoya;
    public GameObject toggleSIAM_IMIDA;
    public GameObject sensorsContainer;

    // Start is called before the first frame update
    void Start()
    {




        // Variables para los IDs de dispositivos (Backend Nuevo)
        string deviceIdSatellite = "6319d587c6520a002bac443f";
        string deviceIdBoya = "Fko-LYcB0U_NvF-sOx7d"; //si
        string deviceIdCamara = "634d862df29d2d0020948ed3";
        string deviceIdSIAM_IMIDA = "4LA0uIcBsYAZ85cQ2mgm"; //si
        string deviceIdAEMET = "6iYcs4cBqOmloIJtmW1j"; //si


        //TODO: newBack :Mando a pedir los tipos de sensores de todos los dispositivos (backend antiguo - bakcend nuevo hay que hacer n peticiones)
        this.GetComponent<RestClient>().getSensorsTypesNamesAndUnits(deviceIdAEMET);
        
        
        // Agregar materiales al diccionario
        materialsDictionary.Add(deviceIdSatellite, materialSatellite);
        materialsDictionary.Add(deviceIdBoya, materialBoya);
        materialsDictionary.Add(deviceIdCamara, materialCamara);
        materialsDictionary.Add(deviceIdSIAM_IMIDA, materialSIAM_IMIDA);
        materialsDictionary.Add(deviceIdAEMET, materialAEMET);

        // Agregar listeners para los cambios en los Toggle
        toggleSatellite.GetComponent<Toggle>().onValueChanged.AddListener((status) => { toggleOnValueChanged(status, deviceIdSatellite, "toggleSatellite"); });
        toggleBoya.GetComponent<Toggle>().onValueChanged.AddListener((status) => { toggleOnValueChanged(status, deviceIdBoya, "toggleBoya"); });
        toggleCamaras.GetComponent<Toggle>().onValueChanged.AddListener((status) => { toggleOnValueChanged(status, deviceIdCamara, "toggleCamaras"); });
        toggleSIAM_IMIDA.GetComponent<Toggle>().onValueChanged.AddListener((status) => { toggleOnValueChanged(status, deviceIdSIAM_IMIDA, "toggleSIAM_IMIDA"); });
        toggleAEMET.GetComponent<Toggle>().onValueChanged.AddListener((status) => { toggleOnValueChanged(status, deviceIdAEMET, "toggleAEMET"); });
        //toggle.GetComponent<Toggle>().onValueChanged.AddListener((status) => { toggleOnValueChanged(status, ""); });



        //////TODO: Toggles y UI Elements fallan as� que hare manual los request. Esto se deber�a borrar luego cuando funcionen
        //toggleOnValueChanged(true, "6319d587c6520a002bac443f", "toggleSatellite");
        //toggleOnValueChanged(true, "Fko-LYcB0U_NvF-sOx7d", "toggleBoya");
        //toggleOnValueChanged(true, "634d862df29d2d0020948ed3", "toggleCamaras");
        //toggleOnValueChanged(true, "4LA0uIcBsYAZ85cQ2mgm", "toggleSIAM_IMIDA");
        //toggleOnValueChanged(true, "6iYcs4cBqOmloIJtmW1j", "toggleAEMET");

    }

    private async void toggleOnValueChanged(bool status, string idTypeSensor, string sensorName)
    {
        //Debug.Log(sensorName + ": " + status);
        if (status)
        {
            //Crear-mostrar objetos de las "boyas" en sus respectivas posiciciones
            //TODO: Gestionar la boya de manera independiente
            //Debug.Log($"{sensorName}- status: {status}    ----- ID: {idTypeSensor}");

            //REQUEST LAST DATA ON SENSORS - ASYNC
            this.GetComponent<RestClient>().GetSensorsLastValuesDataGeneric(idTypeSensor);
            ///REQUEST LOCATIONS
            var result = await this.GetComponent<RestClient>().GetSensorsLocationsGeneric(idTypeSensor);
            if (result == null)
                return;

            CoordinateConverter converter = GetComponentInChildren<CoordinateConverterMono>().getCoordinateConverter();

            //Debug.Log("REQUEST IN PROCESS - Sensors : " + result.locations.Count);
            GameObject[] sensores = new GameObject[result.locations.Count];

            ///TODO: Si es una boya, solo poner la que tiene latitud cero

            for (int i = 0; i < result.locations.Count; i++)
            {
                //Debug.Log((   double)result.locations[i].lat + " - " + (double)result.locations[i].lon);

                var originalCoordinates = new double[3] { (double)result.locations[i].lat, (double)result.locations[i].lon, (double)result.locations[i].altitude };

                //"6319d587c6520a002bac443f_CDT10_37.65888_-0.77854_0"
                //"6319d587c6520a002bac443f_CDT10_37.65888_-0.77854_0"

                //https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/userHardSensors/6319d587c6520a002bac443f/data2?sensor_names=Clorofila,Transparencia,S%C3%B3lidos%20en%20suspensi%C3%B3n&fromDate=1678711184758&toDate=1681303184758&location_ids=6319d587c6520a002bac443f_CDT10_37.65888_-0.77854_0&group=location
                //https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/userHardSensors/Fko-LYcB0U_NvF-sOx7d/data2?sensor_names=Temperatura%20Agua&fromDate=1678538430327&toDate=1681130430327

                String locationId = String.Format("{0}_Profundidad: {1}m_{2}_{3}_{1}",
                    result.locations[i]._id.Split('_')[0] + "_" + result.locations[i]._id.Split('_')[1],
                    result.locations[i].altitude,
                    result.locations[i].lat,
                    result.locations[i].lon * -1);

                Vector3 posCorrecta = converter.TransformarCoordenadas(originalCoordinates);
                sensores[i] = Instantiate(SensorSphere);
                sensores[i].GetComponent<SensorTags>().locationId = result.locations[i]._id;
                sensores[i].transform.position = posCorrecta;
                try
                {
                    sensores[i].GetComponent<Renderer>().material = new Material(materialsDictionary[idTypeSensor]);
                }
                catch (Exception e)
                {
                    //Debug.LogError("ERROR MATEERIAL RENDERER: " + e.Message);
                    sensores[i].GetComponent<Renderer>().material = new Material(materialAEMET);
                }
                sensores[i].transform.parent = sensorsContainer.transform;
                //Debug.Log("Name: " + result.locations[i].name + "-- Coordenadas: " + posCorrecta);
            }
            sensoresDictionary.Add(sensorName, sensores);
        }
        else
        {
            try
            {
                DestroyAllObjects(sensoresDictionary[sensorName]);
                sensoresDictionary.Remove(sensorName);
            }
            catch (Exception) { } //If it's null
        }


    }




    // Update is called once per frame
    void Update()
    {



        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("P: ");

        }
        else if (Input.GetKey(KeyCode.O))
        {
            Debug.Log("O: ");

        }
        else if (Input.GetKey(KeyCode.I))
        {
            Debug.Log("I: ");

        }
        else if (Input.GetKey(KeyCode.U))
        {
            Debug.Log("U: ");

        }
        else if (Input.GetKey(KeyCode.Y))
        {
            Debug.Log("Y: ");

        }
        else if (Input.GetKey(KeyCode.T))
        {
            Debug.Log("T: ");

        }
        else if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("R: ");

        }

    }





    private void DestroyAllObjects(GameObject[] objects)
    {
        int size = objects.Length;
        for (int i = 0; i < size; i++)
            DestroyImmediate(objects[i]);
    }




}

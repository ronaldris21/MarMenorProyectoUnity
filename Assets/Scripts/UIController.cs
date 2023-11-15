using Assets;
using DLL_Models_Petitions.Models.RespuestasPeticiones;
using SmartLagoonApiCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using Assets.Code.Utils;
using System.Runtime.InteropServices;
using Assets.CodeUtils;
using DLL_Models_Petitions.Models;

public class UIController : MonoBehaviour
{
    [SerializeField] ActionBasedController leftController;
    [SerializeField] ActionBasedController rightController;
    [SerializeField] GameObject map;
    [SerializeField] GameObject sensorsContainer;
    [SerializeField] const float triggerThreshold = 0.98f;
    [SerializeField] CoordinateConverterMono coordinateConverterMono;
    [SerializeField] GameObject buoyPrefab;
    [SerializeField] GameObject meteoPrefab;
    [SerializeField] GameObject cameraPrefab;
    [SerializeField] GameObject satellitePrefab;
    [SerializeField] GameObject aemetPrefab;
    [SerializeField] GameObject imidaPrefab;
    [SerializeField] GameObject scrollView;
    [SerializeField] GameObject SensorDataPanel;
    [SerializeField] GameObject togglerMeteo;
    [SerializeField] GameObject togglerCamera;
    [SerializeField] GameObject togglerSatellite;
    [SerializeField] GameObject togglerAemet;
    [SerializeField] GameObject togglerBuoy;
    [SerializeField] GameObject togglerImida;
    [SerializeField] GameObject buttonImida;
    [SerializeField] GameObject buoySensorsTogglePanel;
    [SerializeField] GameObject horizontalSelector;
    [SerializeField] GameObject togglerBuoy0m;
    [SerializeField] GameObject togglerBuoy0Dot5m;
    [SerializeField] GameObject togglerBuoy1m;
    [SerializeField] GameObject togglerBuoy1Dot5m;
    [SerializeField] GameObject togglerBuoy2m;
    [SerializeField] GameObject togglerBuoy2Dot5m;
    [SerializeField] GameObject togglerBuoy3m;
    [SerializeField] GameObject togglerBuoy4m;
    [SerializeField] GameObject togglerBuoy5m;
    [SerializeField] GameObject togglerBuoy6m;
    [SerializeField] GameObject togglerBuoy6Dot5m;
    [SerializeField] Sprite chlorophyllIcon;
    [SerializeField] Sprite conductivityIcon;
    [SerializeField] Sprite humidityIcon;
    [SerializeField] Sprite oxygenConcentrationIcon;
    [SerializeField] Sprite oxygenSaturationIcon;
    [SerializeField] Sprite precipitacionIcon;
    [SerializeField] Sprite pressureIcon;
    [SerializeField] Sprite suspendedSolidsIcon;
    [SerializeField] Sprite transparencyIcon;
    [SerializeField] Sprite turbidityIcon;
    [SerializeField] Sprite waterFlowIcon;
    [SerializeField] Sprite waterLevelIcon;
    [SerializeField] Sprite waterVelocityIcon;
    [SerializeField] Sprite windDirectionIcon;
    [SerializeField] Sprite windDistanceIcon;
    [SerializeField] Sprite windVelocityIcon;
    [SerializeField] GameObject analyticsController;

    private bool leftTriggerPressed = false;
    private bool rightTriggerPressed = false;
    private RaycastHit hit;
    private GameObject scrollViewContent;
    private List<GameObject> instantiatedBuoys = new List<GameObject>();
    private List<GameObject> instantiatedMeteos = new List<GameObject>();
    private List<GameObject> instantiatedCameras = new List<GameObject>();
    private List<GameObject> instantiatedSatellites = new List<GameObject>();
    private List<GameObject> instantiatedAemets = new List<GameObject>();
    private List<GameObject> instantiatedImidas = new List<GameObject>();
    private UGS_Analytics analytics;
    private string idTypeSensor;
    private string idMeteoSensor = "5faa5cd478d639320cb86fa9";
    private string idCameraSensor = "634d862df29d2d0020948ed3";
    private string idSatelliteSensor = "6319d587c6520a002bac443f";
    private string idAemetSensor = "6iYcs4cBqOmloIJtmW1j";
    private string idBuoySensor = "Fko-LYcB0U_NvF-sOx7d";
    private string idImidaSensor = "4LA0uIcBsYAZ85cQ2mgm";
    public string idBuoySensor0m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 0m_37.70940_-0.78552_0";
    private string idBuoySensor0Dot5m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 0.5m_37.70940_-0.78552_-0.5";
    private string idBuoySensor1m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 1m_37.70940_-0.78552_-1";
    private string idBuoySensor1Dot5m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 1.5m_37.70940_-0.78552_-1.5";
    private string idBuoySensor2m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 2m_37.70940_-0.78552_-2";
    private string idBuoySensor2Dot5m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 2.5m_37.70940_-0.78552_-2.5";
    private string idBuoySensor3m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 3m_37.70940_-0.78552_-3";
    private string idBuoySensor4m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 4m_37.70940_-0.78552_-4";
    private string idBuoySensor5m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 5m_37.70940_-0.78552_-5";
    private string idBuoySensor6m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 6m_37.70940_-0.78552_-6";
    private string idBuoySensor6Dot5m = "Fko-LYcB0U_NvF-sOx7d_Profundidad: 6.5m_37.70940_-0.78552_-6.5";
    private bool loadMeteo;
    private bool loadCamera;
    private bool loadSatellites;
    private bool loadAemet;
    private bool loadBuoy;
    private bool loadImida;
    private bool activeMeteo;
    private bool activeCamera;
    private bool activeSatellite;
    private bool activeAemet;
    private bool activeBuoy;
    private bool activeImida;
    private string grayLightHex = "#979797";
    private Color grayLight;
    private string grayDarkHex = "#555F73";
    private Color graydark;

    private String idLocationSelected;
    private bool isBuoy;

    


    //TODO: RIS BORRAR CAMBIOS DE ANTONIO
    //// Location data structures
    //private struct deviceLocationData
    //{
    //    public string _id;
    //    public string name;
    //    public string location;
    //    public double? lat;
    //    public double? lon;
    //    public double? altitude;
    //}





    //private List<deviceLocationData> devicesLocationData = new List<deviceLocationData>();
    //private Dictionary<string, List<deviceLocationData>> devicesLocationDataByDeviceType = new Dictionary<string, List<deviceLocationData>>();

    // Sensor data structures
    private struct indicatorData
    {
        public string sensor_type;
        public double? value;
        public long timestamp;
    }
    private List<indicatorData> lastIndicatorsData = new List<indicatorData>();
    private Dictionary<string, List<indicatorData>> lastIndicatorsDataByDeviceType = new Dictionary<string, List<indicatorData>>();
    
    private string selectedDevice;
    private Transform selectedHitTransform;
    private List<indicatorData> selectedDeviceList;   


    //TODO: Mover a ControlManager.cs y llamar desde ahí
    private void Awake()
    {
        // Call the Sensor Type Name from backend for later displayment
        scrollViewContent = scrollView.transform.Find("Viewport").transform.Find("Content").gameObject;
        ///TODO: newBack: Hacer con todos los Device ID
        this.GetComponent<RestClient>().getSensorsTypesNamesAndUnits(idAemetSensor);
    }

    private void OnEnable()
    {
        this.transform.parent.gameObject.GetComponent<ActionManager>().controls.Enable();
    }

    void Start()
    {
        analytics = analyticsController.GetComponent<UGS_Analytics>();
        //activeMeteo = togglerMeteo.GetComponent<Toggle>().isOn;
        //activeCamera = togglerCamera.GetComponent<Toggle>().isOn;
        //activeSatellite = togglerSatellite.GetComponent<Toggle>().isOn;
        //activeAemet = togglerAemet.GetComponent<Toggle>().isOn;
        //activeBuoy = togglerBuoy.GetComponent<Toggle>().isOn;
        //activeImida = togglerImida.GetComponent<Toggle>().isOn;
    }

    public void ScrollViewSetActiveTrue(bool refreshCanvasLocation)
    {
        scrollView.SetActive(true);
        this.GetComponent<UIControllerChartsAndRangeData>().ActivateFunctionality(idLocationSelected,selectedHitTransform, isBuoy, refreshCanvasLocation); //not async- parallel
    }
    public void ScrollViewSetActiveFalse()
    {
        scrollView.SetActive(false);
        this.GetComponent<UIControllerChartsAndRangeData>().DesactivateFunctionality();
    }

    #region useToggle by Sensor Type
    public void useToggleMeteo(bool tog)
    {

        idTypeSensor = idMeteoSensor;

        if(loadMeteo == false)
        {
            loadLocationData(idTypeSensor, meteoPrefab, instantiatedMeteos);
            loadMeteo = true; // load location only once
        }

        if(!activeMeteo)
        {
            activateObjects(instantiatedMeteos);
            activeMeteo = true;
        }
        else
        {
            deactivateObjects(instantiatedMeteos);
            ScrollViewSetActiveFalse();
            activeMeteo = false;
        }
    }

    public void useToggleCamera(bool tog)
    {
        //ANALYTICS
        analytics.toggleCamara();
        //

        idTypeSensor = idCameraSensor;

        if(loadCamera == false)
        {
            loadLocationData(idTypeSensor, cameraPrefab, instantiatedCameras);
            loadCamera = true;
        }

        if(!activeCamera)
        {
            activateObjects(instantiatedCameras);
            activeCamera = true;
        }
        else
        {
            deactivateObjects(instantiatedCameras);
            ScrollViewSetActiveFalse();
            activeCamera = false;
        }
    }

    public void useToggleSatellite(bool tog)
    {
        //ANALYTICS
        analytics.toggleSatelite();
        //

        idTypeSensor = idSatelliteSensor;

        if(loadSatellites == false)
        {
            loadLocationData(idTypeSensor, satellitePrefab, instantiatedSatellites);
            loadSatellites = true;
        }

        if(!activeSatellite)
        {
            activateObjects(instantiatedSatellites);
            activeSatellite = true;
        }
        else
        {
            deactivateObjects(instantiatedSatellites);
            ScrollViewSetActiveFalse();
            activeSatellite = false;
        }
    }

    public void useToggleAemet(bool tog)
    {
        //ANALYTICS
        analytics.toggleAEMET();
        //

        idTypeSensor = idAemetSensor;

        if(loadAemet == false)
        {
            loadLocationData(idTypeSensor, aemetPrefab, instantiatedAemets);
            loadAemet = true;
        }

        if(!activeAemet)
        {
            activateObjects(instantiatedAemets);
            activeAemet = true;
        }
        else
        {
            deactivateObjects(instantiatedAemets);
            ScrollViewSetActiveFalse();
            activeAemet = false;
        }
    }

    public void useToggleBuoy(bool tog)
    {
        //ANALYTICS
        analytics.toggleBUOY();
        //

        idTypeSensor = idBuoySensor;

        if(loadBuoy == false)
        {
            loadBuoyLocationData(idTypeSensor, aemetPrefab);
            loadBuoy = true;
        }

        if(!activeBuoy)
        {
            activateObjects(instantiatedBuoys);
            activeBuoy = true;
        }
        else
        {
            deactivateObjects(instantiatedBuoys);
            ScrollViewSetActiveFalse();
            activeBuoy = false;
        }

    }

    public void useToggleImida(bool tog)
    {
        //ANALYTICS
        analytics.toggleSIAM_IMIDA();
        //

        idTypeSensor = idImidaSensor;

        if(loadImida == false)
        {
            loadLocationData(idTypeSensor, imidaPrefab, instantiatedImidas);
            loadImida = true;
        }

        if(!activeImida)
        {
            activateObjects(instantiatedImidas);
            activeImida = true;
        }
        else
        {
            deactivateObjects(instantiatedImidas);
            ScrollViewSetActiveFalse();
            activeImida = false;
        }
    }

    // TODO: useButtonImida: borrar? 
    /*public void useButtonImida(bool tog)
    {
        idTypeSensor = idImidaSensor;

        if (loadImida == false)
        {
            loadLocationData(idTypeSensor, imidaPrefab, instantiatedImidas);
            loadImida = true;
        }

        if (!activeImida)
        {
            activateObjects(instantiatedImidas);
            activeImida = true;
            Image buttonImage = buttonImida.transform.Find("Normal").transform.Find("Background").gameObject.GetComponent<Image>();
            changeColorButton(buttonImage, grayLightHex);
        }
        else
        {
            deactivateObjects(instantiatedImidas);
            ScrollViewSetActiveFalse();
            activeImida = false;
            Image buttonImage = buttonImida.transform.Find("Normal").transform.Find("Background").gameObject.GetComponent<Image>();
            changeColorButton(buttonImage, grayDarkHex);
        }
    }*/

    private void changeColorButton(Image buttonImage, string colorHex)
    {
        Color outColor;
        ColorUtility.TryParseHtmlString(colorHex, out outColor);
        buttonImage.color = outColor;

    }
    #endregion

    #region useToggle Buoy by Depth
    public void useToggleBuoy0m(bool tog)
    {
        idLocationSelected = idBuoySensor0m;
        retrieveBuoyLastData(idBuoySensor0m);
        ShowScrollView(scrollViewContent,false);
    }

    public void useToggleBuoy0Dot5m(bool tog)
    {
        idLocationSelected = idBuoySensor0Dot5m;
        retrieveBuoyLastData(idBuoySensor0Dot5m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy1m(bool tog)
    {
        idLocationSelected = idBuoySensor1m;
        retrieveBuoyLastData(idBuoySensor1m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy1Dot5m(bool tog)
    {
        idLocationSelected = idBuoySensor1Dot5m;
        retrieveBuoyLastData(idBuoySensor1Dot5m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy2m(bool tog)
    {
        idLocationSelected = idBuoySensor2m;
        retrieveBuoyLastData(idBuoySensor2m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy2Dot5m(bool tog)
    {
        idLocationSelected = idBuoySensor2Dot5m;
        retrieveBuoyLastData(idBuoySensor2Dot5m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy3m(bool tog)
    {
        idLocationSelected = idBuoySensor3m;
        retrieveBuoyLastData(idBuoySensor3m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy4m(bool tog)
    {
        idLocationSelected = idBuoySensor4m;
        retrieveBuoyLastData(idBuoySensor4m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy5m(bool tog)
    {
        idLocationSelected = idBuoySensor5m;
        retrieveBuoyLastData(idBuoySensor5m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy6m(bool tog)
    {
        idLocationSelected = idBuoySensor6m;
        retrieveBuoyLastData(idBuoySensor6m);
        ShowScrollView(scrollViewContent, false);
    }

    public void useToggleBuoy6Dot5m(bool tog)
    {
        idLocationSelected = idBuoySensor6Dot5m;
        retrieveBuoyLastData(idBuoySensor6Dot5m);
        ShowScrollView(scrollViewContent, false);
    }

    #endregion

    private async void loadLocationData(string idTypeSensor, GameObject prefab, List<GameObject> instantiatedDevices)
    {
        // ask the last values of the sensors - not async, so it prepares data for later use
        this.GetComponent<RestClient>().GetSensorsLastValuesDataGeneric(idTypeSensor);
        var result = await this.GetComponent<RestClient>().GetSensorsLocationsGeneric(idTypeSensor);
        if (result == null)
            return;

        DestroyAllObjects(instantiatedDevices);
        string deviceType = translateDeviceIdToType(idTypeSensor);
        int index = 0;

        CoordinateConverter converter = GetComponentInChildren<CoordinateConverterMono>().getCoordinateConverter();
        ValidMapLimitsVerifier mapLimitsVerifier = this.GetComponent<ValidMapLimitsVerifier>();

        foreach (DLL_Models_Petitions.Models.Location deviceLocation in result.locations)
        {

           

            //Debug.Log(deviceLocation.lat);
            //Debug.Log(deviceLocation.lon);
            double lat = deviceLocation.lat.Value;
            double lon = deviceLocation.lon.Value;
            double alt = deviceLocation.altitude.Value;
            double[] p1 = new double[3] { lat, lon, alt };
            Vector3 q1 = converter.TransformarCoordenadas(p1);
            //Debug.Log("Coordenadas originales: " + $"[{p1[0]},{p1[1]}]");
            //Debug.Log("Coordenadas transformadas: " + q1);


            if (mapLimitsVerifier.PointInMap(q1))
            {
                GameObject instantiatedDevice = Instantiate(prefab, new Vector3(q1.x, 5.0f, q1.z), Quaternion.identity);

                // Place ground devices at floor level
                if ((idTypeSensor == idMeteoSensor) ||
                    (idTypeSensor == idCameraSensor) ||
                    (idTypeSensor == idAemetSensor) ||
                    (idTypeSensor == idImidaSensor))
                {


                    //TODO: hacer RigidBody -> Frozen XZ + Activar gravedad
                    Vector3 instantiatedDevicePos = instantiatedDevice.transform.position;
                    Vector3 mapPos = map.transform.position;
                    Vector3 dist = mapPos - instantiatedDevicePos;
                    dist = new Vector3(Mathf.Abs(dist.x), Mathf.Abs(dist.y), Mathf.Abs(dist.z));
                    Vector3 projection = Vector3.Project(dist, instantiatedDevice.transform.up);
                    instantiatedDevice.transform.position = new Vector3(instantiatedDevicePos.x,
                                                                        instantiatedDevicePos.y - projection.y,
                                                                        instantiatedDevicePos.z);
                }

                instantiatedDevice.transform.SetParent(sensorsContainer.transform);
                instantiatedDevice.AddComponent<SensorTags>();
                instantiatedDevice.GetComponent<SensorTags>().locationId = deviceLocation._id;
                Debug.Log(deviceLocation._id);
                instantiatedDevices.Add(instantiatedDevice);
                index++;
            }
        }
    }

    private async void loadBuoyLocationData(string idTypeSensor, GameObject prefab)
    {
        // There is only one buoy but it has different sensors according to their altitude.
        // Each sensor is treated as a different device. Obviously, all of the has the
        // same latitude and logitude
        // See https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/UserHardSensorslocations/Fko-LYcB0U_NvF-sOx7d

        // ask the last values of the buoys - not async, so it prepares data for later use
        this.GetComponent<RestClient>().GetSensorsLastValuesDataGeneric(idTypeSensor);

        var result = await this.GetComponent<RestClient>().GetSensorsLocationsGeneric(idTypeSensor);
        if (result==null)
        {
            return;

        }
        DestroyAllObjects(instantiatedBuoys);
        string deviceType = translateDeviceIdToType(idTypeSensor);
        int index = 0;
        CoordinateConverter converter = GetComponentInChildren<CoordinateConverterMono>().getCoordinateConverter();
        ValidMapLimitsVerifier mapLimitsVerifier = this.GetComponent<ValidMapLimitsVerifier>();
        List<Double[]> posLatLonArray = new List<Double[]>();

        //TODO: RONALD HACER ESTA FUNCION Y HACER QUE FUNCIONE DINAMICAMENTE PARA LOS DEMAS
        //result.locations = result.locations.Sort((l1,l2) => l1.altitude.CompareTo(y.)).ToList();
        result.locations = result.locations.OrderByDescending(l=>l.altitude).ToList();
        foreach (DLL_Models_Petitions.Models.Location deviceLocation in result.locations)
        {
            

            double lat = deviceLocation.lat.Value;
            double lon = deviceLocation.lon.Value;
            double alt = deviceLocation.altitude.Value;

            int printCount = posLatLonArray.FindAll(pos => (pos[0] == lat && pos[1] == lon)).Count;
            bool isSameLatLon = printCount > 0;
            if(!isSameLatLon)
            {

                double[] p1 = new double[3] { lat, lon, alt };
                Vector3 q1 = converter.TransformarCoordenadas(p1);

                // Prefab buoy
                if (mapLimitsVerifier.PointInMap(q1))
                {
                    GameObject instantiatedDevice = Instantiate(buoyPrefab, new Vector3(q1.x, q1.y, q1.z), Quaternion.identity);
                    // Object info
                    instantiatedDevice.transform.SetParent(sensorsContainer.transform);
                    instantiatedDevice.AddComponent<SensorTags>();
                    instantiatedDevice.GetComponent<SensorTags>().locationId = deviceLocation._id;
                    instantiatedBuoys.Add(instantiatedDevice);
                    index++;

                    // Print just one buoy per lat and lon
                    double[] pos = new double[2];
                    pos[0] = lat;
                    pos[1] = lon;
                    posLatLonArray.Add(pos);
                }
            }
        }
    }

    private void activateObjects(List<GameObject> instantiatedDevices)
    {
        analytics.buttonAllDevice();
        foreach (GameObject instantiatedDevice in instantiatedDevices)
        {
            instantiatedDevice.SetActive(true);
        }
    }

    private void deactivateObjects(List<GameObject> instantiatedDevices)
    {
        analytics.buttonHideAllDevice();
        foreach (GameObject instantiatedDevice in instantiatedDevices)
        {
            instantiatedDevice.SetActive(false);
        }
    }

    void Update()
    {
        leftTriggerPressed = this.transform.parent.gameObject.GetComponent<ActionManager>().leftTriggerInput > triggerThreshold;
        rightTriggerPressed = this.transform.parent.gameObject.GetComponent<ActionManager>().rightTriggerInput > triggerThreshold;

        if(leftTriggerPressed)
        {
            leftController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out RaycastHit leftRaycastHit);
            Vector3 raycasHitPoint = leftRaycastHit.point;
            
            if((raycasHitPoint.x != 0f) || (raycasHitPoint.y != 0f) || (raycasHitPoint.z != 0f))
            {
                hitSuccesfull(leftRaycastHit);
            }
        }
        
        else if(rightTriggerPressed)
        {
            rightController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out RaycastHit rightRaycastHit);
            Vector3 raycasHitPoint = rightRaycastHit.point;

            if ((raycasHitPoint.x != 0f) || (raycasHitPoint.y != 0f) || (raycasHitPoint.z != 0f))
            {
                hitSuccesfull(rightRaycastHit);
            }
        }
    }

    private void hitSuccesfull(RaycastHit hit)
    {
        Debug.Log(hit.collider.tag);
        //this.GetComponent<PopNotification>().callNotification("Funciona", "Vaia por dios que cosas");



        if (hit.collider.tag == "ChartsAndRangeDataClose")
        {
            Debug.Log("ChartsAndRangeDataClose");
            this.GetComponent<UIControllerChartsAndRangeData>().DesactivateFunctionality();
            return;
        }

        selectedDevice = hit.collider.tag;
        selectedHitTransform = hit.transform;

        if (selectedDevice != "Buoy")
        {
            isBuoy = false;
            //buoySensorsTogglePanel.SetActive(false); ///UI ANTIGUA //TODO: BORRAR

        }
        else
        {
            isBuoy = true;
            //ScrollViewSetActiveFalse();
            //buoySensorsTogglePanel.SetActive(true); /// TODO: UI ANTIGUAM BORRAR
        }
        retrieveDevicesLastData(hit);
        ShowScrollView(scrollViewContent);
        ///TODO: DEFINIR DEEPTH SELECCIONADA!

    }

    private void retrieveDevicesLastData(RaycastHit hit)
    {
        idLocationSelected = hit.transform.gameObject.GetComponent<SensorTags>().locationId;
        String deviceTypeId = Singleton.Instance.sensorsIdLocationToDeviceTypeDictionary[idLocationSelected];
        SensorType[] sensorTypes = Singleton.Instance.sensoresDataTypes_byLocationSensorType[deviceTypeId];
        string sensoresParametros = String.Join(",", sensorTypes.Select(s => s.name).ToArray());

        lastIndicatorsData = Singleton.Instance.sensorsLatestDataDictionary[deviceTypeId].data
                                    .Where(c => c.location_id == idLocationSelected)
                                    .Select(c => new  indicatorData{
                                        sensor_type = c.sensor_type,
                                        value = c.value,
                                        timestamp = c.timestamp,
                                    }).ToList();

        // from SensorType.name -> SensorType object ()
        Dictionary<string, SensorType> dictionaryTipoSensor_a_Unidad = new Dictionary<string, SensorType>();
        foreach (var item in sensorTypes)
        {
            dictionaryTipoSensor_a_Unidad.Add(item.name, item);
        }

        foreach (var item in lastIndicatorsData)
        {
            String datoSensor = item.sensor_type + ": " + item.value + " " + dictionaryTipoSensor_a_Unidad[item.sensor_type].units;
            //Debug.Log(datoSensor);
        }

        // Store every list of structs indicatorData (with sensor_type and its value) in a
        // Dictionary<DeviceType,list of struct>
        storeLastDataInDict(idLocationSelected, lastIndicatorsData);
    }

    private void retrieveBuoyLastData(string idLocation)
    {
        //TODO: FALLA QUE sensoresDataTypes_byLocationSensorType NO TIENE DATOS!!!!! ENTONCES HAY QUE LLAMAR AL API DE RESTCLIENT QUE RETORNE ESOS DATOS ANTES PARA PODERLOS USARLOS LUEGO
        String sensorTypeId = Singleton.Instance.sensorsIdLocationToDeviceTypeDictionary[idLocation];
        var sensores = Singleton.Instance.sensoresDataTypes_byLocationSensorType[sensorTypeId];
        string sensoresParametros = String.Join(",", sensores.Select(s => s.name).ToArray());

        lastIndicatorsData = Singleton.Instance.sensorsLatestDataDictionary[sensorTypeId].data
                                    .Where(c => c.location_id == idLocation)
                                    .Select(c => new  indicatorData{
                                        sensor_type = c.sensor_type,
                                        value = c.value,
                                    }).ToList();
        //Debug.Log(lastIndicatorsData.Count);
        
        SensorType[] descripcionTiposSensores = Singleton.Instance.sensoresDataTypes_byLocationSensorType[sensorTypeId];
        Dictionary<string, SensorType> dictionaryTipoSensor_a_Unidad = new Dictionary<string, SensorType>();
        foreach (var item in descripcionTiposSensores)
        {
            dictionaryTipoSensor_a_Unidad.Add(item.name, item);
        }

        foreach (var item in lastIndicatorsData)
        {
            String datoSensor = item.sensor_type + ": " + item.value + " " + dictionaryTipoSensor_a_Unidad[item.sensor_type].units;
            //Debug.Log(datoSensor);
        }

        // Store every list of structs indicatorData (with sensor_type and its value) in a
        // Dictionary<DeviceType,list of struct>
        storeLastDataInDict(idLocation, lastIndicatorsData);
    }


    private void storeLastDataInDict(string idLocation, List<indicatorData> lastIndicatorsData)
    {
        //Backend antiguo
        //string idLocationShort = idLocation.Substring(0, idMeteoSensor.Length); //PROBLEM!!!! id() - alias - la_lon_depth
        //string deviceId = translateDeviceIdToType(idLocationShort);

        string deviceIdHASH = Singleton.Instance.sensorsIdLocationToDeviceTypeDictionary[idLocation];
        string deviceId = translateDeviceIdToType(deviceIdHASH);


        lastIndicatorsDataByDeviceType[deviceId] = lastIndicatorsData;
    }

    private string translateDeviceIdToType(string id)
    {
        string deviceType = "";

        if(id == idMeteoSensor)
        {
            deviceType = "Meteo";
        }
        else if(id == idCameraSensor)
        {
            deviceType = "Camera";
        }
        else if(id == idSatelliteSensor)
        {
            deviceType = "Satellite";
        }
        else if(id == idAemetSensor)
        {
            deviceType = "Aemet";
        }
        else if(id == idBuoySensor)
        {
            deviceType = "Buoy";
        }
        else if(id == idImidaSensor)
        {
            deviceType = "Imida";
        }

        return deviceType;
    }

    private string translateDeviceTypeToId(string deviceType)
    {
        string id = "";
        
        if(deviceType == "Meteo")
        {
            id = idMeteoSensor;
        }
        else if(deviceType == "Camera")
        {
            id = idCameraSensor;
        }
        else if(deviceType == "Satellite")
        {
            id = idSatelliteSensor;
        }
        else if(deviceType == "Aemet")
        {
            id = idAemetSensor;
        }
        else if(deviceType == "Buoy")
        {
            id = idBuoySensor;
        }
        else if(deviceType == "Imida")
        {
            id = idImidaSensor;
        }

        return id;
    }


    // Function called by GameObject UI/PanelCheckbox/ToggleBoya
    private void ShowScrollView(GameObject scrollViewContent, bool refreshCanvasLocation=true)
    {
        // Delete indicators info from previous queries
        foreach (Transform child in scrollViewContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        ScrollViewSetActiveTrue(refreshCanvasLocation);
        
        selectedDeviceList = lastIndicatorsDataByDeviceType[selectedDevice];

        for (int i = 0; i < selectedDeviceList.Count; i++)
        {
            string sensorType = selectedDeviceList[i].sensor_type;
            double? value = selectedDeviceList[i].value;

            // Instatiate new text box to show the name and value of each device indicator.
            // There's already a text box placed in ScrollView/Viewport/Content named
            // "PanelDatosSensor"
            GameObject newSensorDataPanel = Instantiate(SensorDataPanel);
            newSensorDataPanel.transform.SetParent(scrollViewContent.transform, false);
            
            // Place the text box in the same location as PanelDatosSensor. Automatically
            // is placed below
            RectTransform SensorDataPanelTransform = SensorDataPanel.GetComponent<RectTransform>();
            RectTransform newSensorDataPanelTransform = newSensorDataPanel.GetComponent<RectTransform>();
            newSensorDataPanelTransform.anchorMin = SensorDataPanelTransform.anchorMin;
            newSensorDataPanelTransform.anchorMax = SensorDataPanelTransform.anchorMax;
            newSensorDataPanelTransform.anchoredPosition = SensorDataPanelTransform.anchoredPosition;
            newSensorDataPanelTransform.sizeDelta = SensorDataPanelTransform.sizeDelta;



            // Show the name and value of each device indicator
            GameObject vertical = SensorDataPanel.transform.Find("vertical").gameObject;

            TMP_Text DatosSensorTextTMP = vertical.transform.Find("DatosSensorTextTMPValue").gameObject.GetComponent<TMP_Text>();
            string newSensorType = this.GetComponent<UnitPolishers>().polishSensorType(sensorType);
            String unit = this.GetComponent<UnitPolishers>().PolishUnit(sensorType);

            DatosSensorTextTMP.text = newSensorType + ": " + ((float)Math.Round((double)value,2)).ToString() + " "+ unit;
            

            TMP_Text DatosSensorTextTMPfecha = vertical.transform.Find("DatosSensorTextTMPDate").gameObject.GetComponent<TMP_Text>();
            DatosSensorTextTMPfecha.text = TimeStampToDateFormat(selectedDeviceList[i].timestamp);

            // Assign icon
            this.GetComponent<UnitPolishers>().assignIcon(SensorDataPanel, sensorType);
        }
        SensorDataPanel.SetActive(true);
    }

    private string TimeStampToDateFormat(long timestamp)
    {
            long unixDate = 1297380023295;
        DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

        return date.ToShortDateString();
    }

    public void showAllDevices()
    {
        if(activeMeteo == false)
        {
            useToggleMeteo(false);
        }
        if (activeCamera == false)
        {
            useToggleCamera(false);
        }
        if (activeSatellite == false)
        {
            useToggleSatellite(false);
        }
        if (activeAemet == false)
        {
            useToggleAemet(false);
        }
        if (activeBuoy == false)
        {
            useToggleBuoy(false);
        }
        if (activeImida == false)
        {
            useToggleImida(false);
        }
    }

    public void hideAllDevices()
    {
        if (activeMeteo == true)
        {
            useToggleMeteo(true);
        }
        if (activeCamera == true)
        {
            useToggleCamera(true);
        }
        if (activeSatellite == true)
        {
            useToggleSatellite(true);
        }
        if (activeAemet == true)
        {
            useToggleAemet(true);
        }
        if (activeBuoy == true)
        {
            useToggleBuoy(true);
        }
        if (activeImida == true)
        {
            useToggleImida(true);
        }
    }

        private void DestroyAllObjects(GameObject[] objects)
    {
        int size = objects.Length;
        for (int i = 0; i < size; i++)
            DestroyImmediate(objects[i]);
        objects = new GameObject[0];
    }


    private void DestroyAllObjects(List<GameObject> objects)
    {
        int size = objects.Count;
        for (int i = 0; i < size; i++)
            DestroyImmediate(objects[i]);
        objects.Clear();
    }


    private void OnDisable()
    {
        this.transform.parent.gameObject.GetComponent<ActionManager>().controls.Disable();
    }

}

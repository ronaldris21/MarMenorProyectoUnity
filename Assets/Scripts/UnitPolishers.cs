using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.CodeUtils
{
    public class UnitPolishers : MonoBehaviour
    {
        [SerializeField]  Sprite chlorophyllIcon;
        [SerializeField]  Sprite conductivityIcon;
        [SerializeField]  Sprite humidityIcon;
        [SerializeField]  Sprite oxygenConcentrationIcon;
        [SerializeField]  Sprite oxygenSaturationIcon;
        [SerializeField]  Sprite precipitacionIcon;
        [SerializeField]  Sprite pressureIcon;
        [SerializeField]  Sprite suspendedSolidsIcon;
        [SerializeField]  Sprite transparencyIcon;
        [SerializeField]  Sprite turbidityIcon;
        [SerializeField]  Sprite waterFlowIcon;
        [SerializeField]  Sprite waterLevelIcon;
        [SerializeField]  Sprite waterVelocityIcon;
        [SerializeField]  Sprite windDirectionIcon;
        [SerializeField]  Sprite windDistanceIcon;
        [SerializeField]  Sprite windVelocityIcon;
        [SerializeField]  Sprite windTemperatureIcon;
        [SerializeField]  Sprite waterTemperatureIcon;
        [SerializeField]  Sprite temperatureIcon;
        [SerializeField]  Sprite solarRadiationIcon;
        [SerializeField]  Sprite groundTemperatureIcon;
        [SerializeField]  Sprite presureSeaLevelIcon;
        [SerializeField]  Sprite precipitationTemperatureIcon;
        [SerializeField]  Sprite visibilityIcon;


        [SerializeField] GameObject dataPanel;
        [SerializeField] GameObject devicename;
        [SerializeField] GameObject deviceCat;


        [SerializeField] Sprite buoyImage;
        [SerializeField] Sprite camaraImage;
        [SerializeField] Sprite sateliteImage;
        [SerializeField] Sprite imidaImage;
        [SerializeField] Sprite aemetImage;

        public string PolishUnit(string sensorType)
        {
            string unit = "";

            //Debug.Log("POLISH UNIT: Sensor: " + sensorType);
            if (sensorType == "velocity" || sensorType == "vv" || sensorType == "wind_velocity")
            {
                unit = "m/s";
            }
            else if (sensorType == "discharge" || sensorType == "cubic_meter")
            {
                unit = "m³";
            }
            else if (sensorType == "level" || sensorType == "ts" || sensorType == "prec")
            {
                unit = "m";
            }
            else if (sensorType == "pres_nmar" || sensorType == "pres" || sensorType == "presión" || sensorType == "pres_nmar")
            {
                unit = "mbar";
            }
            else if (sensorType == "dv" || sensorType == "wind_direction")
            {
                unit = "°";
            }
            else if (sensorType == "precipitation" || sensorType == "prec" || sensorType == "millimeter")
            {
                unit = "mm";
            }
            else if (sensorType == "temperature" || sensorType == "Temperatura Aire" || sensorType == "Temperatura Agua" || sensorType == "ta" || sensorType == "tpr" || sensorType == "ts" || sensorType == "celsius")
            {
                unit = "°C";
            }
            else if (sensorType == "humidity" || sensorType == "hr" || sensorType == "Humedad Relativa")
            {
                unit = "%";
            }
            else if (sensorType == "Presión Vapor" || sensorType == "presión vapor")
            {
                unit = "kPa";
            }
            else if (sensorType == "Concentración O2" || sensorType == "milligram-per-liter")
            {
                unit = "mg/L";
            }
            else if (sensorType == "Saturación O2" || sensorType == "percent")
            {
                unit = "%";
            }
            else if (sensorType == "Conductividad" || sensorType == "microsiemens-per-centimeter")
            {
                unit = "µS/cm";
            }
            else if (sensorType == "Turbidez" || sensorType == "NTU")
            {
                unit = "NTU";
            }
            else if (sensorType == "solar_radiation")
            {
                unit = "W";
            }
            else if (sensorType == "vis")
            {
                unit = "km";
            }

            else if (sensorType == "rviento")
            {
                unit = "hm";
            }
            else if (sensorType == "Clorofila")
            {
                unit = "hm";
            }
            else if (sensorType == "stdvv")
            {
                unit = "m/s";
            }
            else if (sensorType == "Sólidos en suspensión")
            {
                unit = "g";
            }
            else
            {
                unit = ""; // Default value if a match is not found
            }

            //Debug.Log("POLISH UNIT: Unit: " + unit);

            return unit;
        }

        public  string polishSensorType(string sensorType)
        {
            //Debug.Log("Antes: "+sensorType);
            if (sensorType == "Clorofila")
            {
                sensorType = "Clorofila";
            }
            else if (sensorType == "Transparencia")
            {
                sensorType = "Transparencia";
            }
            else if (sensorType == "Sólidos en suspensión")
            {
                sensorType = "Sólidos en suspensión";
            }
            else if (sensorType == "Temperatura Aire")
            {
                sensorType = "Temperatura aire";
            }
            else if (sensorType == "Humedad Relativa")
            {
                sensorType = "Humedad relativa";
            }
            else if (sensorType == "Presión Vapor")
            {
                sensorType = "Presión vapor";
            }
            else if (sensorType == "Velocidad Viento")
            {
                sensorType = "Velocidad viento";
            }
            else if (sensorType == "Temperatura Agua")
            {
                sensorType = "Temperatura agua";
            }
            else if (sensorType == "Concentración O2")
            {
                sensorType = "Concentración O2";
            }
            else if (sensorType == "Saturación O2")
            {
                sensorType = "Saturación O2";
            }
            else if (sensorType == "Conductividad")
            {
                sensorType = "Conductividad";
            }
            else if (sensorType == "Turbidez")
            {
                sensorType = "Turbidez";
            }
            else if (sensorType == "discharge")
            {
                sensorType = "Vertido aguas";
            }
            else if (sensorType == "level")
            {
                sensorType = "Nivel agua";
            }
            else if (sensorType == "velocity")
            {
                sensorType = "Velocidad";
            }
            else if (sensorType == "temperature")
            {
                sensorType = "Temperatura";
            }
            else if (sensorType == "precipitation")
            {
                sensorType = "Precipitaciones";
            }
            else if (sensorType == "humidity")
            {
                sensorType = "Humedad";
            }
            else if (sensorType == "solar_radiation")
            {
                sensorType = "Radiación solar";
            }
            else if (sensorType == "wind_velocity")
            {
                sensorType = "Velocidad viento";
            }
            else if (sensorType == "wind_direction")
            {
                sensorType = "Dirección viento";
            }
            else if (sensorType == "prec")
            {
                sensorType = "Precipitaciones";
            }
            else if (sensorType == "vv")
            {
                sensorType = "Velocidad viento";
            }
            else if (sensorType == "dv")
            {
                sensorType = "Dirección viento";
            }
            else if (sensorType == "pres")
            {
                sensorType = "Presión";
            }
            else if (sensorType == "hr")
            {
                sensorType = "Humedad relativa";
            }
            else if (sensorType == "stdvv")
            {
                sensorType = "STD dirección viento";
            }
            else if (sensorType == "ts")
            {
                sensorType = "Temperatura suelo";
            }
            else if (sensorType == "pres_nmar")
            {
                sensorType = "Presión nivel mar";
            }
            else if (sensorType == "ta")
            {
                sensorType = "Temperatura aire";
            }
            else if (sensorType == "tpr")
            {
                sensorType = "Temperatura rocio";
            }
            else if (sensorType == "vis")
            {
                sensorType = "Visibilidad";
            }
            else if (sensorType == "rviento")
            {
                sensorType = "Distancia viento";
            }
            
            return sensorType;
        }



        public string sensorType(string idLocation) {

            string sensor= "";
            string idCameraSensor = "634d862df29d2d0020948ed3";
            string idSatelliteSensor = "6319d587c6520a002bac443f";
            string idAemetSensor = "6iYcs4cBqOmloIJtmW1j";
            string idBuoySensor = "Fko-LYcB0U_NvF-sOx7d";
            string idImidaSensor = "4LA0uIcBsYAZ85cQ2mgm";

            if (idLocation == idCameraSensor) 
            {
                sensor = "CameraSensor";
            }
            else if (idLocation == idSatelliteSensor) 
            {
                sensor = "SatelliteSensor";
            }
            else if (idLocation == idAemetSensor)
            {
                sensor = "AemetSensor";
            }
            else if (idLocation == idBuoySensor)
            {
                sensor = "BuoySensor";
            }
            else if (idLocation == idImidaSensor)
            {
                sensor = "ImidaSensor";
            }

            return sensor;
        }

        public void assignImage(string idLocation)
        {

            String deviceTypeId = Singleton.Instance.sensorsIdLocationToDeviceTypeDictionary[idLocation];

            string idMeteoSensor = "5faa5cd478d639320cb86fa9";
            string idCameraSensor = "634d862df29d2d0020948ed3";
            string idSatelliteSensor = "6319d587c6520a002bac443f";
            string idAemetSensor = "6iYcs4cBqOmloIJtmW1j";
            string idBuoySensor = "Fko-LYcB0U_NvF-sOx7d";
            string idImidaSensor = "4LA0uIcBsYAZ85cQ2mgm";

            var dato = Singleton.Instance.DeviceTypeLocationsDictionary[deviceTypeId].locations.Where(l => l._id == idLocation).First().name;

            if (deviceTypeId == idBuoySensor) //BOYA
            {
               dataPanel.GetComponent<Image>().sprite = buoyImage;
               
                devicename.GetComponent<TextMeshProUGUI>().text = dato.ToString();
                deviceCat.GetComponent<TextMeshProUGUI>().text = "Boya";

            }
            else if (deviceTypeId == idCameraSensor)
            {
                dataPanel.GetComponent<Image>().sprite = camaraImage;
                devicename.GetComponent<TextMeshProUGUI>().text = "CAMARA";
                devicename.GetComponent<TextMeshProUGUI>().text = dato.ToString();
                deviceCat.GetComponent<TextMeshProUGUI>().text = "Camara";

            }
            else if (deviceTypeId == idSatelliteSensor)
            {
                dataPanel.GetComponent<Image>().sprite = sateliteImage;
                devicename.GetComponent<TextMeshProUGUI>().text = "SATELITE";
                devicename.GetComponent<TextMeshProUGUI>().text = dato.ToString();
                deviceCat.GetComponent<TextMeshProUGUI>().text = "Satelite";
            }
            else if (deviceTypeId == idAemetSensor || idMeteoSensor == deviceTypeId)
            {
                dataPanel.GetComponent<Image>().sprite = aemetImage;
                devicename.GetComponent<TextMeshProUGUI>().text = "AEMET";
                devicename.GetComponent<TextMeshProUGUI>().text = dato.ToString();
                deviceCat.GetComponent<TextMeshProUGUI>().text = "AEMET";
            }
            else if (deviceTypeId == idImidaSensor)
            {
                dataPanel.GetComponent<Image>().sprite = imidaImage;
                devicename.GetComponent<TextMeshProUGUI>().text = "SIAM/IMIDA";
                devicename.GetComponent<TextMeshProUGUI>().text = dato.ToString();
                deviceCat.GetComponent<TextMeshProUGUI>().text = "SIAM/IMIDA";
            }

        }

        public void assignIcon(GameObject sensorDataPanel, string sensorType)
        {
            if (sensorType == "Clorofila")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = chlorophyllIcon;
            }
            else if (sensorType == "Transparencia")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = transparencyIcon;
            }
            else if (sensorType == "Sólidos en suspensión")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = suspendedSolidsIcon;
            }
            else if (sensorType == "Temperatura Aire")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windTemperatureIcon; //DONE
            }
            else if (sensorType == "Humedad Relativa")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = humidityIcon;
            }
            else if (sensorType == "Presión Vapor")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = pressureIcon;
            }
            else if (sensorType == "Velocidad Viento")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windVelocityIcon;
            }
            else if (sensorType == "Temperatura Agua")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = waterTemperatureIcon; //DONE
            }
            else if (sensorType == "Concentración O2")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = oxygenConcentrationIcon;
            }
            else if (sensorType == "Saturación O2")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = oxygenSaturationIcon;
            }
            else if (sensorType == "Conductividad")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = conductivityIcon;
            }
            else if (sensorType == "Turbidez")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = turbidityIcon;
            }
            else if (sensorType == "discharge")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = waterFlowIcon;
            }
            else if (sensorType == "level")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = waterLevelIcon;
            }
            else if (sensorType == "velocity")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windVelocityIcon;
            }
            else if (sensorType == "temperature")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = temperatureIcon; //DONE
            }
            else if (sensorType == "precipitation")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = precipitacionIcon;
            }
            else if (sensorType == "humidity")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = humidityIcon;
            }
            else if (sensorType == "solar_radiation")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = solarRadiationIcon; //DONE
            }
            else if (sensorType == "wind_velocity")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windVelocityIcon;
            }
            else if (sensorType == "wind_direction")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windDirectionIcon;
            }
            else if (sensorType == "prec")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = precipitacionIcon;
            }
            else if (sensorType == "vv")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windVelocityIcon;
            }
            else if (sensorType == "dv")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windDirectionIcon;
            }
            else if (sensorType == "pres")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = pressureIcon;
            }
            else if (sensorType == "hr")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = humidityIcon;
            }
            else if (sensorType == "stdvv")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windVelocityIcon;
            }
            else if (sensorType == "ts")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = groundTemperatureIcon; //temperatura suelo
            }
            else if (sensorType == "pres_nmar")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = presureSeaLevelIcon; //presion nivel mar
            }
            else if (sensorType == "ta")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = precipitationTemperatureIcon;
            }
            else if (sensorType == "tpr")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = precipitationTemperatureIcon; // temperatura precipitacio
            }
            else if (sensorType == "vis")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = visibilityIcon; //visibilidad
            }
            else if (sensorType == "rviento")
            {
                sensorDataPanel.transform.Find("ImageSensor").GetComponent<Image>().sprite = windDistanceIcon;
               
            }
        }

        public Sprite GetSensorIcon(string sensorType)
        {
            if (sensorType == "Clorofila")
            {
                return chlorophyllIcon;
            }
            else if (sensorType == "Transparencia")
            {
                return transparencyIcon;
            }
            else if (sensorType == "Sólidos en suspensión")
            {
                return suspendedSolidsIcon;
            }
            else if (sensorType == "Temperatura Aire")
            {
                return windTemperatureIcon;
            }
            else if (sensorType == "Humedad Relativa")
            {
                return humidityIcon;
            }
            else if (sensorType == "Presión Vapor")
            {
                return pressureIcon;
            }
            else if (sensorType == "Velocidad Viento")
            {
                return windVelocityIcon;
            }
            else if (sensorType == "Temperatura Agua")
            {
                return waterTemperatureIcon;
            }
            else if (sensorType == "Concentración O2")
            {
                return oxygenConcentrationIcon;
            }
            else if (sensorType == "Saturación O2")
            {
                return oxygenSaturationIcon;
            }
            else if (sensorType == "Conductividad")
            {
                return conductivityIcon;
            }
            else if (sensorType == "Turbidez")
            {
                return turbidityIcon;
            }
            else if (sensorType == "discharge")
            {
                return waterFlowIcon;
            }
            else if (sensorType == "level")
            {
                return waterLevelIcon;
            }
            else if (sensorType == "velocity")
            {
                return windVelocityIcon;
            }
            else if (sensorType == "temperature")
            {
                return temperatureIcon;
            }
            else if (sensorType == "precipitation")
            {
                return precipitacionIcon;
            }
            else if (sensorType == "humidity")
            {
                return humidityIcon;
            }
            else if (sensorType == "solar_radiation")
            {
                return solarRadiationIcon;
            }
            else if (sensorType == "wind_velocity")
            {
                return windVelocityIcon;
            }
            else if (sensorType == "wind_direction")
            {
                return windDirectionIcon;
            }
            else if (sensorType == "prec")
            {
                return precipitacionIcon;
            }
            else if (sensorType == "vv")
            {
                return windVelocityIcon;
            }
            else if (sensorType == "dv")
            {
                return windDirectionIcon;
            }
            else if (sensorType == "pres")
            {
                return pressureIcon;
            }
            else if (sensorType == "hr")
            {
                return humidityIcon;
            }
            else if (sensorType == "stdvv")
            {
                return windVelocityIcon;
            }
            else if (sensorType == "ts")
            {
                return groundTemperatureIcon;
            }
            else if (sensorType == "pres_nmar")
            {
                return presureSeaLevelIcon;
            }
            else if (sensorType == "ta")
            {
                return precipitationTemperatureIcon;
            }
            else if (sensorType == "tpr")
            {
                return precipitationTemperatureIcon;
            }
            else if (sensorType == "vis")
            {
                return visibilityIcon;
            }
            else if (sensorType == "rviento")
            {
                return windDistanceIcon;
            }

            return chlorophyllIcon;
        }



    }
}

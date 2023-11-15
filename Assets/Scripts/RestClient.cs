using System;
using Newtonsoft.Json;
using DLL_Models_Petitions.Models.RespuestasPeticiones;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.RespuestasPeticiones;
using Assets;
using UnityEngine;
using System.Reflection;
using Microsoft.CSharp;
using Newtonsoft.Json.Linq;
using Ping = System.Net.NetworkInformation.Ping;
using System.Net.NetworkInformation;
using DLL_Models_Petitions.Models;
using System.Threading;


/*
http://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/UserHardSensorsLastValuesByLocations/Fko-LYcB0U_NvF-sOx7d/data
http://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/UserHardSensorsLastValuesByLocations/6iYcs4cBqOmloIJtmW1j/data
http://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/userHardSensors

*/


namespace SmartLagoonApiCalls
{
    public class RestClient:MonoBehaviour
    {

        private String APP_ID = "5f8edc99a3302d002d06241d";

        // Create a custom HttpClientHandler with SSL certificate validation disabled
        HttpClientHandler httpClientHandler = new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Automatic,
            SslProtocols = System.Security.Authentication.SslProtocols.None,
            PreAuthenticate = false,
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                // Always return true to bypass SSL certificate validation
                return true;
            }
        };

        private async Task CheckInternetConnection()
        {
            try
            {
                System.Net.NetworkInformation.Ping myPing = new System.Net.NetworkInformation.Ping();
                String host = "www.google.com";
                byte[] buffer = new byte[32];
                int timeout = 10000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = await myPing.SendPingAsync(host, timeout, buffer, pingOptions); //.SendAsync(host, timeout, buffer, pingOptions);
                Console.WriteLine("Comporbar conexsion");
                if (reply.Status != IPStatus.Success)
                {
                    Debug.Log(reply.Status);
                    //TODO: DESCOMENTAR SI ESTAMOS EN LA UCAM
                    //this.GetComponent<PopNotification>().callNotification("Error de conexion", "No se puede acceder a internet: "+ reply.Status);
                }
            }
            catch (Exception e)
            {
                ControlErrorMessage(e);
                this.GetComponent<PopNotification>().callNotification("Error de conexion", "No se puede acceder a internet");
            }
        }

        private void ControlErrorMessage(Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.InnerException);
        }


        ////FUNCIONA BIEN EN AMBOS BACKEND
        ///TODO: CAMBIAR NOMBRE A DISPOSITIVO!!
        public async Task<Device_LocationsDTO> GetSensorsLocationsGeneric(String idTypeSensor)
        {
            CheckInternetConnection();

            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ///Datos ya cargados en la aplicación
            if (Singleton.Instance.DeviceTypeLocationsDictionary.ContainsKey(idTypeSensor))
                return Singleton.Instance.DeviceTypeLocationsDictionary[idTypeSensor];    
            
            try
            {
                //Sample old
                //https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/UserHardSensorslocations/4LA0uIcBsYAZ85cQ2mgm
                //String url = $"https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/UserHardSensorslocations/{idTypeSensor}";

                ///Sample new
                String url = $"https://heterolistic.ucam.edu/api/user-hard-sensors/{idTypeSensor}/locations";
                Debug.Log("GetSensorsLocationsGeneric");
                Debug.Log(url);
                var result = await httpClient.GetAsync(url);
                String json = await result.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<Device_LocationsDTO>(json);

                Singleton.Instance.DeviceTypeLocationsDictionary.Add(idTypeSensor, datos); ///Guardo los datos en local

                //Guardo los id con los tipos de sensore para buscar luego los tipos de sensores que tienen
                for (int i = 0; i < datos.locations.Count; i++)
                    Singleton.Instance.sensorsIdLocationToDeviceTypeDictionary.Add(datos.locations[i]._id, idTypeSensor);


                return datos;
            }
            catch (Exception e)
            {
                this.GetComponent<PopNotification>().callNotification("Problemas del servidor", "No es posible obtener las ubicaciones de los dispositivos", time: 5);
                ControlErrorMessage(e);
                return null;
            }
        }



        public async Task<Sensor_DATADTO> GetSensorsLastValuesDataGeneric(String idTypeSensor)
        {
            CheckInternetConnection();
            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                ///new URL
                String url = $"https://heterolistic.ucam.edu/api/user-hard-sensors/{idTypeSensor}/data/latest-values";

                ///old URL
                //String url = $"https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/UserHardSensorsLastValuesByLocations/{idTypeSensor}/data";
                Debug.Log("GetSensorsLastValuesDataGeneric");
                Debug.Log(url);
                var result = await httpClient.GetAsync(url);
                String json = await result.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<Sensor_DATADTO>(json);


                if (Singleton.Instance.sensorsLatestDataDictionary.ContainsKey(idTypeSensor))
                    Singleton.Instance.sensorsLatestDataDictionary.Remove(idTypeSensor);
                
                Singleton.Instance.sensorsLatestDataDictionary.Add(idTypeSensor, datos); //Cargo datos de los últimos valores

                return datos;
            }
            catch (Exception e)
            {
                this.GetComponent<PopNotification>().callNotification("Problemas del servidor", "No es posible obtener los últimos datos de los sensores", time: 7);

                ControlErrorMessage(e);
                return null;
            }
        }

        

        ///okay newBack
        public async Task<Dictionary<string, SensorType[]>> getSensorsTypesNamesAndUnits(string IdDevice)
        {
            CheckInternetConnection();
            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                Singleton.Instance.sensoresDataTypes_byLocationSensorType = new Dictionary<string, SensorType[]>();
                Singleton.Instance.dictionaryTipoSensor_a_Unidad = new Dictionary<string, SensorType>();
                Singleton.Instance.dictionaryTipoSensor_a_Unidad.Add("", new SensorType() { name = "", units = "" });
                //Singleton.Instance.dictionaryTipoSensor_a_Unidad.Add("level", new SensorType() { name = "level", unit = "meter" });
                //Singleton.Instance.dictionaryTipoSensor_a_Unidad.Add("velocity", new SensorType() { name = "velocity", unit = "meter-per-second" });
                //Singleton.Instance.dictionaryTipoSensor_a_Unidad.Add("rviento", new SensorType() { name = "rviento", unit = "Hectometer" });

                String url = $"https://heterolistic.ucam.edu/api/user-hard-sensors/{IdDevice}/"; ///UNIDADES DEL AEMET SOLAMENTE
                Debug.Log("getSensorsTypesNamesAndUnits");
                Debug.Log(url);
                var result = await httpClient.GetAsync(url);
                String json = await result.Content.ReadAsStringAsync();
                UserHardSensorsDTO userHardSensors = JsonConvert.DeserializeObject<UserHardSensorsDTO>(json);
                String borrar = "";

                int cantSensors = userHardSensors.userHardSensor.sensor_types.Count();
                DLL_Models_Petitions.Models.RespuestasPeticiones.SensorType[] sensorTypes = new DLL_Models_Petitions.Models.RespuestasPeticiones.SensorType[cantSensors];
                for (int j = 0; j < cantSensors; j++)
                {
                    sensorTypes[j] = userHardSensors.userHardSensor.sensor_types[j];
                    if (!Singleton.Instance.dictionaryTipoSensor_a_Unidad.ContainsKey(sensorTypes[j].name))
                    {
                        Singleton.Instance.dictionaryTipoSensor_a_Unidad.Add(sensorTypes[j].name, sensorTypes[j]);
                        borrar += String.Format("{0}={1}\n", sensorTypes[j].name, sensorTypes[j].units);
                    }
                }

                Singleton.Instance.sensoresDataTypes_byLocationSensorType.Add(IdDevice, sensorTypes);

                Debug.Log(borrar);
                return Singleton.Instance.sensoresDataTypes_byLocationSensorType;

            }
            catch (Exception e)
            {
                ControlErrorMessage(e);
                this.GetComponent<PopNotification>().callNotification("Problemas del servidor", "No es posible obtener los datos de las unidades de los sensores", time: 7);
                return null;
            }
        }
         
        public async Task<SensorRangeData> getSensorDataUsingDates(string locationId, string deviceId, string sensorNames, string date1 = "1673974807324", string date2 = "1674579607324" )
        {
            CheckInternetConnection();
            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                SensorRangeData data = new SensorRangeData();

                ///NEW Back
                String URI = $"https://heterolistic.ucam.edu/api/user-hard-sensors/{deviceId}/data?sensorTypeNames={sensorNames}&locationIds={locationId}&fromDate={date1}&toDate={date2}";

                ///old Back
                //String URI = "https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/userHardSensors/6iYcs4cBqOmloIJtmW1j/data2?fromDate=1673974807324&toDate=1674579607324&group=location&location_ids=6iYcs4cBqOmloIJtmW1j_7012C - CARTAGENA_37.60111_-0.98778_17&sensor_names=prec,vv,dv,pres,hr,stdvv,ts,pres_nmar,ta,tpr,vis,rviento";
                //string URI = $"https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/userHardSensors/{deviceId}/data2?fromDate={date1}&toDate={date2}&group=location&location_ids={locationId}&sensor_names={sensorNames}";
                Debug.Log("getSensorDataUsingDates");
                Debug.Log(URI);


                var response = await httpClient.GetAsync(URI);
                var json = await response.Content.ReadAsStringAsync();



                JObject jsonGlobal = JObject.Parse(json);
                // obtener el objeto "series" por su nombre de propiedad
                JObject seriesObject = ((JObject)jsonGlobal["series"]);


                foreach (JProperty child in seriesObject.Children())
                {
                    string propertyName = child.Name;
                    JToken propertyValue = child.Value;

                    Console.WriteLine("IDLOCATION: " + propertyName);
                    data.idLocation = propertyName;
                    foreach (JProperty sensorParam in propertyValue.Children())
                    {
                        string sensorParamName = sensorParam.Name;
                        JToken sensorParamValue = sensorParam.Value;
                        Console.WriteLine(sensorParamName);


                        List<TimeValueData> dataList = new List<TimeValueData>();
                        foreach (JToken paramValue in sensorParamValue.Children())
                        {
                            //value, date, timestamp
                            long timestamp = (long)paramValue[0];
                            DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
                            TimeValueData dato = new TimeValueData((float)paramValue[1], date, timestamp);
                            dataList.Add(dato);
                        }

                        data.paramsTypeUnitName.Add(sensorParamName);
                        data.paramTimeValueDataDictionary.Add(sensorParamName, dataList);

                    }
                    Console.WriteLine();
                }

                return data;
            }
            catch (Exception e)
            {
                this.GetComponent<PopNotification>().callNotification("Problemas del servidor", "No es posible obtener los datos del rango de fechas solicitado", time: 7);
                ControlErrorMessage(e);
                return null;
            }
        }



        //​http://sabuda.grc.upv.es/api/hard-sensors/weather-data?sensor=temperature,precipitation,humidity,solar_radiation,wind_velocity,wind_direction&fromDate=1673974807324&toDate=1674579607324&physicalStationId=AEMET-7031X&predictionSource=AEMET
        //TODO: Make this with custom data - Create DTO
        public async Task<ApplicationDataDTO> getSensorPredictionsDataValues(String date1 = "1673974807324", String date2 = "1674579607324", String id_stattionSeleccionada = "AEMET-7031X", String prediction = "AEMET")
        {
            CheckInternetConnection();
            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                //EJEMPLO: http://sabuda.grc.upv.es/api/hard-sensors/weather-data?sensor=temperature,precipitation,humidity,solar_radiation,wind_velocity,wind_direction&fromDate=1673974807324&toDate=1674579607324&physicalStationId=AEMET-7031X&predictionSource=AEMET

                String url = $"​http://sabuda.grc.upv.es/api/hard-sensors/weather-data?sensor=temperature,precipitation,humidity,solar_radiation,wind_velocity,wind_direction&fromDate={date1}&toDate={date2}&physicalStationId={id_stattionSeleccionada}&predictionSource={prediction}";
                Debug.Log("getSensorPredictionsDataValues");
                Debug.Log(url);
                var response = await httpClient.GetAsync(url);

                Debug.Log(response.ToString());

                var result = await response.Content.ReadAsStringAsync();
                Debug.Log(result.ToString());
                ApplicationDataDTO datos = JsonConvert.DeserializeObject<ApplicationDataDTO>(result);
                return datos;
            }
            catch (Exception e)
            {
                ControlErrorMessage(e);

                return new ApplicationDataDTO();
            }
        }
    
    
    }
}



/*
 * 
 * 
 * 
 * 
 * 
 *
 *

//TODO: Descripcion de esto

-Estructura de datos ya explicada 
web:
referencia a las peticiones HTTP, que se crearon los diccionarios. GET, 
Dejar claro que se ha desarrollado un cliente http. Newstonsoft Y httpClient de DOTNET. (CREAR LOS MODELOS)

Accesos  a los end points de smart laggon y hacer peticiones a ellos

getSensorsTypesNamesAndUnits NOMBRAR LAS PETICIONES, para explicar el flujo de datos, Secuencia de las peticiones en las que se hacen.
DICTIONARIOS




 *
 *
 */
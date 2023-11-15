using DLL_Models_Petitions.Models;
using DLL_Models_Petitions.Models.RespuestasPeticiones;
using System;
using System.Collections.Generic;

namespace Assets
{

    public class Singleton
    {
		public ApplicationDataDTO ApplicationDataDTO { get; set; } //Model for global Query, in case we want to know in future version, all the "device types" (buoys, satellites,...) dynamically
		public Dictionary<string, Device_LocationsDTO> DeviceTypeLocationsDictionary { get; set; }
        public Dictionary<string, Sensor_DATADTO> sensorsLatestDataDictionary { get; set; }
        public Dictionary<string, string> sensorsIdLocationToDeviceTypeDictionary { get; set; }
		public Dictionary<string, string> sensorsTypesId { get; set; }
		public Dictionary<string, SensorType[]> sensoresDataTypes_byLocationSensorType { get; set; } // buoys = "Fko-LYcB0U_NvF-sOx7d" -> SensorTypes[] of buoys
		public Dictionary <string, SensorType> dictionaryTipoSensor_a_Unidad { set; get; } //sensorType.name = "Clorofila" -> sensorType ({"params":[],"name":"Clorofila","unit":"microgram-per-liter","params_":[]})



		private Singleton()
		{
			DeviceTypeLocationsDictionary = new Dictionary<string, Device_LocationsDTO>();
			sensorsLatestDataDictionary = new Dictionary<string, Sensor_DATADTO>();
            sensoresDataTypes_byLocationSensorType = new Dictionary<string, SensorType[]>();
			sensorsIdLocationToDeviceTypeDictionary = new Dictionary<string, string>();

			//TODO: dictionario Unit to Unit a mostrar en UI  polish Function
            //['milligram-per-liter', 'meter', 'gram', 'celsius', 'percent', 'kilopascal', 'meter-per-second', 'celsius', 'milligram-per-liter', 'percent', 'microsiemens-per-centimeter', 'microgram-per-liter', 'NTU', 'cubic_meter', 'celsius', 'millimeter', 'percent', 'watt', 'meter-per-second', 'degree', 'millimeter', 'meter-per-second', 'degree', 'millibar', 'percent', 'meter-per-second', 'celsius', 'millibar', 'celsius', 'celsius', 'kilometer', 'microgram-per-liter']



			////Eleccion del backend antiguo o nuevo:
			
	    }






        #region Patron Singleton

        private static Singleton _instance;
		public static Singleton Instance
		{
			get
			{
				if(_instance==null)
					_instance = new Singleton();
				return _instance;
			}
		}

        #endregion
    }
}

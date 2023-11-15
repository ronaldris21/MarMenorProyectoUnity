using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace DLL_Models_Petitions.Models.RespuestasPeticiones
{

    public class UserHardSensorsDTO
    {
        public UserHardSensor userHardSensor { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Alert
    {
        public string min { get; set; }
        public string max { get; set; }
    }

    

    public class SensorType
    {
        public string color { get; set; }
        public Alert alert { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public object forecast { get; set; }
        public int? id { get; set; }
        public string units { get; set; }
    }

    public class UserHardSensor
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<SensorType> sensor_types { get; set; }
        public string application_id { get; set; }
        public string type { get; set; }
        public bool? active { get; set; }
        public string _id { get; set; }
        public string _index { get; set; }
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Models_Petitions.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Application
    {
        public Logo? logo { get; set; }
        public List<HardSensor>? hardSensors { get; set; }
        public List<UserHardSensor>? userHardSensors { get; set; }
        public List<string>? timeSeries { get; set; }
        public string? _id { get; set; }
        public string? userId { get; set; }
        public string? name { get; set; }
        public string? color { get; set; }
        public bool? @private { get; set; }
        public string? defaultTimeSeries { get; set; }
        public int? __v { get; set; }
        public bool? softSensorQuestionsActive { get; set; }
        public bool? softSensorsActive { get; set; }
        public List<StationsSelected>? stationsSelected { get; set; }
        public string? contextCategoryId { get; set; }
    }

}

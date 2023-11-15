using DLL_Models_Petitions.Models.RespuestasPeticiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Models_Petitions.Models
{
    public class UserHardSensor
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public bool? active { get; set; }

        public List<object>? telegramChatIds { get; set; }
        public string? _id { get; set; }
        public List<SensorType>? sensorTypes { get; set; }
        public string? application { get; set; }
        public string? type { get; set; }
        public string? esIndex { get; set; }
        public string? esPhysicalStationsAlias { get; set; }
        public int? __v { get; set; }
    }
}

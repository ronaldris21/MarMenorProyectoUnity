using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Models_Petitions.Models.RespuestasPeticiones
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Sensor_DATADTO
    {
        public List<Datum> data { get; set; }
    }
    public class Datum
    {
        public string location_id { get; set; }
        public string name { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string alt { get; set; }
        public string sensor_type { get; set; }
        public double? value { get; set; }
        public long timestamp { get; set; }
    }

    


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.RespuestasPeticiones
{
    public class SensorValuesInTimestamp
    {
        Dictionary<String, SensorDataValues> valores { get; set; }
    }

    public class SensorDataValues
    {
        public string SeriesId { get; set; }
        public Dictionary<string, List<List<object>>> Data { get; set; }
    }
}

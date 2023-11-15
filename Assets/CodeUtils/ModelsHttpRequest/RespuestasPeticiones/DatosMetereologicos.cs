using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Models_Petitions.Models.RespuestasPeticiones
{
    class DatosMetereologicos
    {
        public Counters? counters { get; set; }
        public LatestValues? latest_values { get; set; }
    }

    public class Counters
    {
        public double temperature { get; set; }
        public double precipitation { get; set; }
        public double humidity { get; set; }
        public double solar_radiation { get; set; }
        public double wind_velocity { get; set; }
        public double wind_direction { get; set; }
    }

    public class Humidity
    {
        public long timestamp { get; set; }
        public string? sensor_type { get; set; }
        public int value { get; set; }
        public string? physical_station_id { get; set; }
        public string? id_medicion { get; set; }
    }

    public class LatestValues
    {
        public Temperature? temperature { get; set; }
        public Precipitation? precipitation { get; set; }
        public Humidity? humidity { get; set; }
        public SolarRadiation? solar_radiation { get; set; }
        public WindVelocity? wind_velocity { get; set; }
        public WindDirection? wind_direction { get; set; }
    }

    public class Precipitation
    {
        public long? timestamp { get; set; }
        public string? sensor_type { get; set; }
        public int? value { get; set; }
        public string? physical_station_id { get; set; }
        public string? id_medicion { get; set; }
    }


    public class SolarRadiation
    {
        public long? timestamp { get; set; }
        public string? sensor_type { get; set; }
        public int? value { get; set; }
        public string? physical_station_id { get; set; }
        public string? id_medicion { get; set; }
    }

    public class Temperature
    {
        public long? timestamp { get; set; }
        public string? sensor_type { get; set; }
        public double? value { get; set; }
        public string? physical_station_id { get; set; }
        public string? id_medicion { get; set; }
    }

    public class WindDirection
    {
        public long? timestamp { get; set; }
        public string? sensor_type { get; set; }
        public int? value { get; set; }
        public string? physical_station_id { get; set; }
        public string? id_medicion { get; set; }
    }

    public class WindVelocity
    {
        public long? timestamp { get; set; }
        public string? sensor_type { get; set; }
        public double? value { get; set; }
        public string? physical_station_id { get; set; }
        public string? id_medicion { get; set; }
    }
}

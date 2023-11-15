using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Models_Petitions.Models
{
    public class Counters
    {
        public double? temperature { get; set; }
        public double? precipitation { get; set; }
        public double? humidity { get; set; }
        public double? solar_radiation { get; set; }
        public double? wind_velocity { get; set; }
        public double? wind_direction { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Models_Petitions.Models
{
    public class StationsSelected
    {
        public Coordinates? coordinates { get; set; }
        public string? _id { get; set; }
        public string? id { get; set; }
        public string? location { get; set; }
        public string? prediction { get; set; }
    }
}

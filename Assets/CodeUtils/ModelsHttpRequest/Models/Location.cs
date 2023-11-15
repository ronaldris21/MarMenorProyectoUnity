using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Models_Petitions.Models
{
    public class Location
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public double? lat { get; set; }
        public double? lon { get; set; }
        public double? altitude { get; set; }
    }
}

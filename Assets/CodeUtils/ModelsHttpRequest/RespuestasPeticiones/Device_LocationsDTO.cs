using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Models_Petitions.Models.RespuestasPeticiones
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    
    public class Device_LocationsDTO
    {
        public List<Location> locations { get; set; }
    }


    //public class Location
    //{
    //    public string _id { get; set; }
    //    public string name { get; set; }
    //    public string location { get; set; }
    //    public double? lat { get; set; }
    //    public double? lon { get; set; }
    //    public double? altitude { get; set; }
    //}






}

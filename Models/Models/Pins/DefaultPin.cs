
namespace AuthJWT.Models
{
    public class DefaultPin : EntityPin
    {
        public double Lat { get; set; } // coordinates on map 
        public double Lng { get; set; } // coordinates on map
        public string Name { get; set; } // Name of Pin
        public string Address { get; set; }
    }
}

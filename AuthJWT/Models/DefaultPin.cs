
namespace AuthJWT.Models
{
    public class DefaultPin : Entity
    {
        public double Lat { get; set; } // coordinates on map 
        public double Lng { get; set; } // coordinates on map
        public string Name { get; set; } // Name of Pin
        public string LocationDescription { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; } = 0; // if it's live building
        public string Region { get; set; }
        
    }
}

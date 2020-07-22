using AuthJWT.Models.Images;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthJWT.Models
{
    [Table("ProblemPins")]
    public class ProblemPin : DefaultPin
    {
        public string PinSvgUrl { get; } = "http://localhost:54968/PinPublicImages/problemPin.svg"; // pin color on map 
        public ICollection<ProblemImages> Images { get; set; } // pin Images
        public string ProblemDescription { get; set; }
    }
}

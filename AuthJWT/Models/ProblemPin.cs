using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthJWT.Models
{
    [Table("ProblemPins")]
    public class ProblemPin : DefaultPin
    {
        public string PinSvgUrl { get; } = FileServerPath + "problemPin.svg"; // pin color on map 
        public ICollection<ImageCustom> Images { get; set; } // pin Images
        public string ProblemDescription { get; set; }
        public string GetPinSvgUrl()
        { 
            return PinSvgUrl;
        }

    }
}

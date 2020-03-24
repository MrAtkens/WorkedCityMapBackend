using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models
{
    public class ProblemPin : DefaultPin
    {
        private string HexColor { get; set; } = "#f3423"; // pin color on map 

        [Required, MaxLength(400)]
        public string ProblemDescription { get; set; }

        public string GetPinColor()
        {
            return HexColor;
        }
    }
}

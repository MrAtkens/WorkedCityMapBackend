using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.DTOs
{
    public class ProblemPinDTO
    {
        [Required]
        public Guid UserKeyId { get; set; } // user ip in bcrypt
        [Required]
        public double Lat { get; set; } // coordinates on map 
        [Required]
        public double Lng { get; set; } // coordinates on map
        [Required, MaxLength(50)]
        public string Name { get; set; } // Name of Pin
        [Required, MaxLength(150)]
        public string Description { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public int BuildingNumber { get; set; } // if it's live building
        [Required]
        public string Region { get; set; }
        [Required]
        public List<string> ImagesPath { get; set; } // pin Images
        public DateTime CreationDate { get; set; } = DateTime.Now;
        private string HexColor { get; set; } = "#f3423"; // pin color on map 

        [Required, MaxLength(400)]
        public string ProblemDescription { get; set; }

        public string GetPinColor()
        {
            return HexColor;
        }
    }
}

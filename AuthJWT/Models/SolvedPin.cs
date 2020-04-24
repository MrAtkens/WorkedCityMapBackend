using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthJWT.Models
{
    public class SolvedPin : DefaultPin
    {
        private string HexColor { get; set; } = "#00FF00"; // pin color on map 

        public string Team { get; set; }
        public string Report { get; set; } // description for solved problem

        public string ProblemDescription { get; set; }

        public ICollection<ImageCustom> Images { get; set; } // pin Images

        [NotMapped]
        public List<string> SolvedPinImagesPath { get; set; }
        public DateTime SolveDate { get; } = DateTime.Now;
        public Guid SolvedModerator { get; set; }

        public string GetPinColor()
        {
            return HexColor;
        }
    }
}

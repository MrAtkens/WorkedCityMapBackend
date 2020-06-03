using AuthJWT.Models.Images;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthJWT.Models
{
    public class SolvedPin : ProblemPin
    {
        public string Team { get; set; }
        public string Report { get; set; } // description for solved problem
        public ICollection<SolvedImages> SolvedImages { get; set; } // pin Images
        public DateTime SolveDate { get; } = DateTime.Now;
        public Guid SolvedModerator { get; set; }

    }
}

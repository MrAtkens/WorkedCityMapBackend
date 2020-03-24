using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.DTOs
{
    public class SolvedPinDTO
    {
        public string Team { get; set; }
        public string Report { get; set; } // description for solved problem
        public List<string> SolvedPinImagesPath { get; set; }
    }
}

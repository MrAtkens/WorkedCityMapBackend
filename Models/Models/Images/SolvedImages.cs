using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models.Images
{
    public class SolvedImages : ProblemImages
    {
        public Guid? solvedPinId { get; set; }
        public SolvedPin SolvedPin { get; set; }
    }
}

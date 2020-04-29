using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models.Images
{
    public class ProblemImages : ImageCustom
    {
        public Guid? problemPinId { get; set; }
        public ProblemPin problemPin { get; set; }
    }
}

using AuthJWT.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.DTOs.Pins
{
    public class EditProblemPinDTO : ProblemPinDTO
    {
        public Guid ModeratorId { get; set; }
        public string ModeratorLogin { get; set; }
    }
}

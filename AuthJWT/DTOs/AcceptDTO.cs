using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.DTOs
{
    public class AcceptDTO
    {
        public Guid Id { get; set; }
        public Guid ModeratorId { get; set; }
    }
}

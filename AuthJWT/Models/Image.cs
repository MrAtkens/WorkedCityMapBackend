using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models
{
    public class Image
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ImagePath { get; set; }
        public string Alt { get; set; }
    }
}

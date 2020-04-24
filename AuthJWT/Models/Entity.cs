using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models
{
    public class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ModeratorId { get; set; }
        public Guid UserKeyId { get; set; } // user ip in bcrypt
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }

        protected static string FileServerPath = "http://localhost:54968/PinPublicImages/";

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.System
{
    public class VerificationCode
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}

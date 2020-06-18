using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.DTOs
{
    public class AuthUserDTO
    {
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Phone { get; set; }
        
        [Required]
        [MaxLength(16)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}

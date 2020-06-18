using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.DTOs
{
    public class RegistartionUserDTO
    {
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(16)]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [MaxLength(6)]
        [MinLength(6)]
        public string VerificationCode { get; set; }

    }
}

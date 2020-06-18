using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.DTOs.AuthModerations
{
    public class AdminDTO
    {
        [Required]
        [MaxLength(16)]
        [MinLength(3)]
        public string Login { get; set; }

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(16)]
        [MinLength(8)]
        public string Password { get; set; }


    }
}

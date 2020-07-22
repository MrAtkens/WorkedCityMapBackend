using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs.AuthModerations
{
    public class AdminDTO
    {
        [Required]
        [MaxLength(16)]
        [MinLength(4)]
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

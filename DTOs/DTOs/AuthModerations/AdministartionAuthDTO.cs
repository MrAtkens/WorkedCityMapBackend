using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs.AuthModerations
{
    public class AdministartionAuthDTO
    {
        [Required]
        [MaxLength(16)]
        [MinLength(3)]
        public string Login { get; set; }

        [Required]
        [MaxLength(16)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}

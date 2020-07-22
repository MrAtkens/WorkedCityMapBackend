using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs.AuthModerations
{
    public class EditAdministrationDTO
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [MaxLength(16)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}

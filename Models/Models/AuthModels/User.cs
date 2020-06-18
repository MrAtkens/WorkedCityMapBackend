using AuthJWT.Models.AuthModels;
using System.ComponentModel.DataAnnotations;

namespace AuthJWT.Models
{
    public class User : EntityUser
    {
        [Required]
        public string Phone { get; set; }
        public string Email { get; set; } 
        public bool EmailAccept { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace AuthJWT.Models.AuthModels
{
    public class ModeratorEntity : EntityUser
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}

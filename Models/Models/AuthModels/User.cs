using AuthJWT.Models.AuthModels;
namespace AuthJWT.Models
{
    public class User : EntityUser
    {
        public string Phone { get; set; }
        public string Email { get; set; } 
        public bool EmailAccept { get; set; }

    }
}

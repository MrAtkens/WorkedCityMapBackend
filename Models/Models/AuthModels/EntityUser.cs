using System;
using System.ComponentModel.DataAnnotations;

namespace AuthJWT.Models.AuthModels
{
    public class EntityUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public string Token { get; set; }
    }
}

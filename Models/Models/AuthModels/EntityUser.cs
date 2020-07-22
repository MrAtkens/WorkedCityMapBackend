using Models.Models.AuthModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthJWT.Models.AuthModels
{
    public class EntityUser : IEntityUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
    }
}

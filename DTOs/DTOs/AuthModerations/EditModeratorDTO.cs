using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DTOs.AuthModerations
{
    public class EditModeratorDTO : EditAdministrationDTO
    {
        [Required]
        public Guid AdminId { get; set; }
        [Required]
        public string AdminLogin { get; set; }
    }
}

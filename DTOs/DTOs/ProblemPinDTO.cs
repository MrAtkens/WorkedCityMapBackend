using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthJWT.DTOs
{
    public class ProblemPinDTO
    {

        [Required(ErrorMessage = "Координаты широты не установлены")]
        public double Lat { get; set; } // coordinates on map 

        [Required(ErrorMessage = "Координаты долготы не установлены")]
        public double Lng { get; set; } // coordinates on map

        [Required(ErrorMessage = "Вы не вписали название проблемы"), MaxLength(50)]
        public string Name { get; set; } // Name of Pin
        [Required(ErrorMessage = "Вы не описали проблему"), MaxLength(450)]
        public string ProblemDescription { get; set; }
        [Required(ErrorMessage = "Вы не указали адресс")]
        public string Address { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}

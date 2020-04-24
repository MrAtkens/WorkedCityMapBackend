using AuthJWT.Models;
using Microsoft.AspNetCore.Http;
using System;
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
        [Required(ErrorMessage = "Вы не описали место положения маркера"), MaxLength(150)]
        public string LocationDescription { get; set; }
        [Required(ErrorMessage = "Вы не указали улицу")]
        public string Street { get; set; }
        public int BuildingNumber { get; set; } = 0; // if it's live building
        [Required(ErrorMessage = "Вы не указали регион")]
        public string Region { get; set; }
        public string ProblemDescription { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}

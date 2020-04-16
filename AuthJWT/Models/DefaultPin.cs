using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthJWT.Models
{
    public class DefaultPin
    {
        protected static string FileServerPath = "http://localhost:54968/Images/";

        [Required(ErrorMessage = "Идентификатор маркера не установлен")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Идентификатор пользователя не установлен")]
        public Guid UserKeyId { get; set; } // user ip in bcrypt
       
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

        [Required(ErrorMessage = "Не найдены фотографий")]
        public List<Image> Images { get; set; } // pin Images

       [Required(ErrorMessage = "Не обнаружен идентификатор модератора")]
        public Guid ModeratorId { get; set; }
        private string HexColor { get; set; } // pin color on map 
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
    }
}

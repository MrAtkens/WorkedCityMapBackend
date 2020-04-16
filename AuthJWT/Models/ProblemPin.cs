using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthJWT.Models
{
    [Table("ProblemPins")]
    public class ProblemPin : DefaultPin
    {
        public string PinSvgUrl { get; set; } = FileServerPath + "problemPin.svg"; // pin color on map 

        [Required(ErrorMessage = "Вы не вписали данные о проблеме"), MaxLength(400)]
        public string ProblemDescription { get; set; }
        public string GetPinSvgUrl()
        {
            return PinSvgUrl;
        }

    }
}

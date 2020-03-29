using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthJWT.Models
{
    [Table("ProblemPins")]
    public class ProblemPin : DefaultPin
    {
        private string HexColor { get; set; } = "#f3423"; // pin color on map 

        [Required(ErrorMessage = "Вы не вписали данные о проблеме"), MaxLength(400)]
        public string ProblemDescription { get; set; }
        public string GetPinColor()
        {
            return HexColor;
        }

    }
}

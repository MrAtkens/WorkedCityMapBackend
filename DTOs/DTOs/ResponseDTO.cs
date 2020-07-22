using Models.Models.Interfaces;

namespace DTOs.DTOs
{
    public class ResponseDTO
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public virtual IResponse  ResponseData { get; set; }
    }
}

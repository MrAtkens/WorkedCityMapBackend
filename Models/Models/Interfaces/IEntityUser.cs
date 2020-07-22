
using Models.Models.Interfaces;

namespace Models.Models.AuthModels
{
    public interface IEntityUser : IResponse
    {
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}

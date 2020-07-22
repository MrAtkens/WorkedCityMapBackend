using System;

namespace Models.Models.Interfaces
{
    public interface IResponse
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

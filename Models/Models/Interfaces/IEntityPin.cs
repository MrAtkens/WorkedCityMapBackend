using System;

namespace Models.Models.Interfaces
{
    public interface IEntityPin : IResponse
    {
        public Guid ModeratorId { get; set; }
        public Guid UserKeyId { get; set; }

        public string FileServerPath(string url);
    }
}

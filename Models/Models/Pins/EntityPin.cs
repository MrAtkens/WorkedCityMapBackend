using Models.Models.Interfaces;
using System;

namespace AuthJWT.Models
{
    public class EntityPin : IEntityPin
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ModeratorId { get; set; }
        public Guid UserKeyId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
        public string FileServerPath(string url) {
            return url;
        }

    }
}

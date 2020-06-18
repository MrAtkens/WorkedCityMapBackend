using System;

namespace AuthJWT.Models.AuthModels
{
    public class Moderator : ModeratorEntity
    { 
        public int ModeratedPinsCount { get; set; }
        public int AcceptedWorksCount { get; set; }
        public DateTime ModeratorAddedDate { get; set; } = DateTime.Now;
    }
}

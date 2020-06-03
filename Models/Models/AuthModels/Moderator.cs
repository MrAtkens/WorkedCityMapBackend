using System;

namespace AuthJWT.Models.AuthModels
{
    public class Moderator : ModeratorEntity
    { 
        public string ModeratedPinsCount { get; set; }
        public string AcceptedWorksCount { get; set; }
        public DateTime ModeratorAddedDate { get; set; } 
    }
}

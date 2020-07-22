using System;

namespace AuthJWT.Models.AuthModels
{
    public class Moderator : ModeratorEntity
    { 
        public int ModeratedPinsCount { get; set; }
        public int AcceptedWorksCount { get; set; }
    }
}

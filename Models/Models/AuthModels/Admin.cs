using System.Collections.Generic;

namespace AuthJWT.Models.AuthModels
{
    public class Admin : ModeratorEntity
    {
        public List<Moderator> AddedModerators { get; set; }
        public List<Team> AddedTeams { get; set; } 
        public bool IsSuper { get; set; }
    }
}

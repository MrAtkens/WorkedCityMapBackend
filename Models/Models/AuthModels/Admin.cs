﻿using System;

namespace AuthJWT.Models.AuthModels
{
    public class Admin : ModeratorEntity
    {
        public int AddedModerators { get; set; }
        public int AddedTeams { get; set; }
    }
}

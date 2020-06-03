using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.Models.AuthModels
{
    public class Team : EntityUser
    {
        public string Login { get; set; }
        public string TeamName { get; set; }
        public int SolvedWorksCount { get; set; }
        public int WorkersCount { get; set; }
    }
}

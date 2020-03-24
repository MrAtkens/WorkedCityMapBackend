using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.DataAcces
{
    public class ModeratePinContext : DbContext
    {
        public ModeratePinContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<ProblemPin> ProblemPins { get; set; }

    }
}

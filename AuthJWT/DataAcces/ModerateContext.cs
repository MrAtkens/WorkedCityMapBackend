using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AuthJWT.DataAcces
{
    public class ModerateContext : DbContext
    {
        public ModerateContext(DbContextOptions<ModerateContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.Migrate();
        }

        public DbSet<ProblemPin> ModerateProblemPins { get; set; }

    }
}

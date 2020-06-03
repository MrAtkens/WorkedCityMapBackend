using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AuthJWT.DataAcces
{
    public class PinsContext : DbContext
    {
        public PinsContext(DbContextOptions<PinsContext> options) : base(options)
        {
           Database.EnsureCreated();
            //Database.Migrate();
        }

        public DbSet<ProblemPin> ProblemPins { get; set; }
        public DbSet<SolvedPin> SolvedPins { get; set; }

    }
}

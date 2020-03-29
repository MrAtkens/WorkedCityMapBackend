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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProblemPin>().HasData(new ProblemPin
            {
                Name = "kalgf",
                Description = "gsdofkgsodfg",
                ProblemDescription = "dfgksdfgsdfg",
                Lat = 51.5346345,
                Lng = 71.342354,
                Street = "Abay",
                Region = "Karagandy",
                UserKeyId = Guid.NewGuid(),
                ImagesPath = new List<string> { "sdgfa", "dgsdfg" }
            });
        }
    }
}

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolvedPin>().HasData(new SolvedPin
            {
                Name = "Silmar",
                Description = "Kali02",
                ProblemDescription = "Kali02",
                Lat = 51.1246345,
                Lng = 71.232354,
                Street = "Saken",
                Region = "Opel",
                UserKeyId = Guid.NewGuid(),
                ImagesPath = new List<string> { "352346523", "34563456" },
                BuildingNumber = 0,
                ModeratorId = Guid.NewGuid(),
                Report = "gisdjfgoisjdfipg",
                Team = "gsdffhsdfh",
                SolvedPinImagesPath = new List<string> { "634563456", "6523463456"},
                SolvedModerator = Guid.NewGuid()
            });
        }
    }
}

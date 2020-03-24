using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.DataAcces
{
    public class PublicPinContext : DbContext
    {
        public PublicPinContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<ProblemPin> ProblemPins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProblemPin>().HasData(new ProblemPin
            {
                Lat = 71.23434,
                Lng = 51.23415,
                Name = "Test",
                Description = "Test",
                Street = "Saken Seifulin",
                BuildingNumber = 40,
                Region = "SaryArka",
                ModerationAcceptStatus = false,
                SolveStatus = false,
            });;
        }
    }
}

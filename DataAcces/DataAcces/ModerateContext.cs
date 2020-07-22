using AuthJWT.Models;
using AuthJWT.Models.AuthModels;
using Microsoft.EntityFrameworkCore;
using System;

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

        public DbSet<Moderator> Moderators { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(new Admin
             {
               Login = "Admin1234",
               Password = BCrypt.Net.BCrypt.HashPassword("Admin1234").ToString(),
               FirstName = "Admin",
               LastName = "Admin",
               Role = Role.SuperAdmin,
               AddedModerators = 0,
               AddedTeams = 0,
               CreationDate = DateTime.Now
             });
        }
    }
}

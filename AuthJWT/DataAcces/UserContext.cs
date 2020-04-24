using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
namespace AuthJWT.DataAcces
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Username = "Volo",
                Password = "1234"
            });
        }
    }
}

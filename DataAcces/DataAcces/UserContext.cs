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
    }
}

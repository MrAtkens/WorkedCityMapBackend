using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using Models.Models.System;

namespace AuthJWT.DataAcces
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<VerificationCode> Codes { get; set; }
        
    }
}

using AuthJWT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthJWT.DataAcces
{
    public class SolvedPinContext : DbContext
    {
        public SolvedPinContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<SolvedPin> SolvedPins { get; set; }

    }
}

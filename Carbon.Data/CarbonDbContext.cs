using Carbon.Model.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Data
{
    public class CarbonDbContext : DbContext
    {
        public CarbonDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetails>().ToTable("user_details");
        }
    }
}

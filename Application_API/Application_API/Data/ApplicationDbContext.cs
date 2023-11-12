using Microsoft.EntityFrameworkCore;
using Application_API.Models;

namespace Application_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Animals> Animals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Shelters> Shelters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}

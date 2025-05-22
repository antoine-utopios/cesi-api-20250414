using Demo.BruteForce.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.BruteForce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; } = null!;
        public DbSet<LoginAttempt> LoginAttempts { get; set; } = null!;
    }
}

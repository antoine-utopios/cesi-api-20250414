using Exercices.Exo01.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exercices.Exo01.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) { }
    
        public virtual DbSet<LoginAttempt> LoginAttempts { get; set; } = null!;
        public virtual DbSet<AppUser> Users { get; set; } = null!;

    }
}

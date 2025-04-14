using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;

namespace WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Cat> Cats { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}

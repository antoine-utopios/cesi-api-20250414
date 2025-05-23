using Exercices.Exo01.Data;

namespace Exercices.Exo01.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}

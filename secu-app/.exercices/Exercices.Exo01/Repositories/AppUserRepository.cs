using Exercices.Exo01.Data;
using Exercices.Exo01.Entities;

namespace Exercices.Exo01.Repositories
{
    public class AppUserRepository : BaseRepository, IRepository<AppUser, long>
    {
        public AppUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<AppUser> AddAsync(AppUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AppUser?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> UpdateByIdAsync(long id, AppUser entity)
        {
            throw new NotImplementedException();
        }
    }
}

using Exercices.Exo01.Data;
using Exercices.Exo01.Entities;

namespace Exercices.Exo01.Repositories
{
    public class LoginAttemptRepository : BaseRepository, IRepository<LoginAttempt, long>
    {
        public LoginAttemptRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<LoginAttempt> AddAsync(LoginAttempt entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LoginAttempt>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LoginAttempt>> GetAllForSpecificEmailAndAfterDateTimeAsync(string Email, DateTime datetime)
        {
            throw new NotImplementedException();
        }



        public Task<LoginAttempt?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<LoginAttempt> UpdateByIdAsync(long id, LoginAttempt entity)
        {
            throw new NotImplementedException();
        }
    }
}

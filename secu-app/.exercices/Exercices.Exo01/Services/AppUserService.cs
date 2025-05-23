using Exercices.Exo01.Entities;
using Exercices.Exo01.Repositories;

namespace Exercices.Exo01.Services
{
    public class AppUserService
    {
        private readonly IRepository<AppUser, long> _appUserRepository;

        public AppUserService(IRepository<AppUser, long> appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<AppUser?> GetUserByEmail(string email)
        {
            var user = await _appUserRepository.GetUserByEmail(email);
            return user;
        }

        public async Task<bool> IsAccountLocked(string email)
        {
            var user = await GetUserByEmail(email);

            if (user == null)
            {
                return false;
            }

            return user.IsAccountLocked;
        }

        public async Task<bool> LockUserByEmail(string email)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }
            user.IsAccountLocked = true;
            var updatedUser = await _appUserRepository.UpdateByIdAsync(user.Id, user);
            return updatedUser != null;
        }
    }
}

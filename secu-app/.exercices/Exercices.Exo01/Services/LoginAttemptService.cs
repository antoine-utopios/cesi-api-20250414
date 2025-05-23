using Exercices.Exo01.Entities;
using Exercices.Exo01.Repositories;

namespace Exercices.Exo01.Services
{
    public class LoginAttemptService
    {
        private readonly IRepository<LoginAttempt, long> _loginAttemptRepository;

        private const int MaxLoginAttempts = 5;
        public LoginAttemptService(IRepository<LoginAttempt, long> loginAttemptRepository)
        {
            _loginAttemptRepository = loginAttemptRepository;
        }

        public async Task<bool> CreateNewLoginAttemptForEmail(string email)
        {
            var newLoginAttempt = await _loginAttemptRepository.AddAsync(new LoginAttempt
            {
                Email = email,
                AttemptedAt = DateTime.Now
            });

            return newLoginAttempt != null;
        }

        public async Task<bool> HasLessThan5LoginAttempsForLastMinute(string email)
        {
            // On récupère les tentatives de login d'un email de la dernière minute
            var loginAttempts = await ((LoginAttemptRepository)_loginAttemptRepository).GetAllForSpecificEmailAndAfterDateTimeAsync(email, DateTime.Now.AddMinutes(-1));

            // Si on en a au minimum 5, on ne peut pas se connecter
            if (loginAttempts.Count() >= MaxLoginAttempts)
            {
                return false;
            }

            return true;
        }
    }
}

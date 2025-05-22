using System.ComponentModel.DataAnnotations;
using Demo.BruteForce.Data;
using Demo.BruteForce.Entities;
using Demo.BruteForce.Models;

namespace Demo.BruteForce.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public LoginSuccessResponsePayload Login(LoginRequestPayload payload)
        {
            var userFound = _context.Users.FirstOrDefault(x => x.Email == payload.Email);
            if (userFound == null)
            {
                // On gère les exceptions en cas d'utilisateur non trouvé
                throw new Exception("User not found");
            }

            // Est-ce que le compte est bloqué ?
            if (userFound.IsLocked) throw new Exception("User is locked");

            // On vérifie le mot de passe (évidemment avec un système d'encodage)
            if (userFound.Password != payload.Password)
            {
                // On crée une nouvelle entité 'tentative de connexion' dans le but de le stocker en BdD
                var loginAttempt = new LoginAttempt()
                {
                    Email = payload.Email,
                };

                _context.LoginAttempts.Add(loginAttempt);
                _context.SaveChanges();


                // On compte ensuite le nombre de tentatives effectuées depuis une minute pour ce compte
                var nbOfAttempts = _context.LoginAttempts
                    .Where(x => x.Email == payload.Email)
                    .Where(x => x.AttemptedAt > DateTime.UtcNow.AddMinutes(-1))
                    .Count();

                // Si le nombre de tentatives cette dernière minute est supérieur à 5
                if (nbOfAttempts >= 5)
                {
                    // On bloque le compte
                    userFound.IsLocked = true;
                    _context.SaveChanges();
                }

                throw new Exception("Password is incorrect");

            }

            return new LoginSuccessResponsePayload {

                // On génère un vrai token ici
                Token = Guid.NewGuid().ToString()
            };
        }

        public bool UnlockAccount(String Email)
        {
            // On cherche l'utilisateur dans le but de le déverrouiller
            var foundUser = _context.Users.FirstOrDefault(x => x.Email == Email);

            if (foundUser == null) throw new Exception("User not found!");

            foundUser.IsLocked = false;

            // On cherche ses tentatives effectuées précédemment
            var attemptsByUser = _context.LoginAttempts.Where(x => x.Email == Email)
                .ToList();

            // On supprime les tentatives dans le but de libérer de la place en BdD (si demandé)
            _context.LoginAttempts.RemoveRange(attemptsByUser);

            // On sauvegarde les changements
            return _context.SaveChanges() > 0;
            
        }

        public bool Register(RegisterRequestPayload payload)
        {
            // On vérifie que les deux champs de mot de passe concordent
            if (payload.Password != payload.ConfirmPassword)
            {
                throw new Exception("Passwords do not match");
            }

            // On vérifie si l'utilisateur existe déjà
            var userFound = _context.Users.FirstOrDefault(x => x.Email == payload.Email);
            if (userFound != null)
            {
                throw new Exception("User already exists");
            }

            // On crée un nouvel utilisateur
            var newUser = new AppUser()
            {
                Email = payload.Email,
                Password = payload.Password
            };
            _context.Users.Add(newUser);
            return _context.SaveChanges() > 0;
        }


    }
}

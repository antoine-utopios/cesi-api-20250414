using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Exercices.Exo01.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Exercices.Exo01.Services
{
    public class AuthService
    {
        private readonly AppUserService _appUserService;
        private readonly LoginAttemptService _loginAttemptService;

        private readonly JwtSettings _jwtSettings;

        public AuthService(AppUserService appUserService, LoginAttemptService loginAttemptService, IOptions<JwtSettings> jwtSettings)
        {
            _appUserService = appUserService;
            _loginAttemptService = loginAttemptService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginSuccessResponsePayload> Login(LoginRequestPayload payload)
        {
            // ON récupère l'utilisateur avec l'email fourni
            var user = await _appUserService.GetUserByEmail(payload.Email);
            if (user == null) throw new Exception("User not found");

            // On vérifie si le compte de l'utilisateur est verrouillé ou non
            if (await _appUserService.IsAccountLocked(user.Email)) throw new Exception("Account is locked");

            // On test le mot de passe
            if (!BCrypt.Net.BCrypt.Verify(payload.Password, user.Password))
            {
                // Si le mot de passe n'est pas le bon, on va ajouter une nouvelle tentative de connexion infructueuse
                await _loginAttemptService.CreateNewLoginAttemptForEmail(user.Email);

                // Si l'on a déjà plus de 5 tentatives de connexion infructueuses, on va verrouiller le compte
                if (await _loginAttemptService.HasLessThan5LoginAttempsForLastMinute(user.Email))
                {
                    // On va lever une exception pour indiquer que le mot de passe est incorrect
                    throw new Exception("Bad credentials!");
                } else
                {
                    await _appUserService.LockUserByEmail(user.Email);
                    throw new Exception("Account is locked");

                }

            }

            // On construit les claims pour le token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // On récupère la clé depuis notre config (secrets.json)
            var key = new SymmetricSecurityKey(Convert.FromBase64String(_jwtSettings.Key));
            
            // On signe les credentials
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // On construit le token à partir des informations de notre configuration (pour éviter encore une fois les typos)
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: credentials
            );

            // On renvoie le token de connexion au contrôleur
            return new LoginSuccessResponsePayload
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };
        }
    }
}

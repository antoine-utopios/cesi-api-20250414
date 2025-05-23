namespace Exercices.Exo01.Entities
{
    public class AppUser
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsAccountLocked { get; set; } = false;
    }
}

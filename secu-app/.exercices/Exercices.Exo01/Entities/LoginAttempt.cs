namespace Exercices.Exo01.Entities
{
    public class LoginAttempt
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;
    }
}

namespace Demo.BruteForce.Entities
{
    public class LoginAttempt
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;
    }
}

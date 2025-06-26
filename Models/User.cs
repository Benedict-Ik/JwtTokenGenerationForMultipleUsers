namespace JwtTokenGenerationForMultipleUsers.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Store hashed passwords in production
    }
}

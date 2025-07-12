namespace JwtTokenGenerationForMultipleUsers.Models.DTOs
{
    public record AuthRequest(string Username = "", string Password = "");
}

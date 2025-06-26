using JwtTokenGenerationForMultipleUsers.Models;

namespace JwtTokenGenerationForMultipleUsers
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(string username, string password);
    }
}

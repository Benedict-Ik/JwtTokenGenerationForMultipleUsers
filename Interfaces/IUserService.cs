using JwtTokenGenerationForMultipleUsers.Models;

namespace JwtTokenGenerationForMultipleUsers.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(string username, string password);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> CreateUserAsync(User user, string rawPassword);
        Task<bool> DeleteUserAsync(int id);
    }
}

using JwtTokenGenerationForMultipleUsers.Data;
using JwtTokenGenerationForMultipleUsers.Interfaces;
using JwtTokenGenerationForMultipleUsers.Models;
using JwtTokenGenerationForMultipleUsers.Utility;
using Microsoft.EntityFrameworkCore;

namespace JwtTokenGenerationForMultipleUsers.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null) return null;

                bool isValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);
                return isValid ? user : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsersAsync: {ex.Message}");
                return Enumerable.Empty<User>();
            }
        }

        public async Task<User?> CreateUserAsync(User user, string rawPassword)
        {
            if (user == null || string.IsNullOrWhiteSpace(rawPassword))
                throw new ArgumentException("User and raw password must be provided.");

            // Check if user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            try
            {
                user.PasswordHash = PasswordHelper.HashPassword(rawPassword);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateUserAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteUserAsync: {ex.Message}");
                return false;
            }
        }
    }
}

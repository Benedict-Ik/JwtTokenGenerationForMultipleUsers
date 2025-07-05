using JwtTokenGenerationForMultipleUsers.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtTokenGenerationForMultipleUsers.Utility
{
    public class JwtService
    {
        private readonly JwtSettings _options;

        public JwtService(IOptions<JwtSettings> options)
        {
            _options = options.Value ;
            //_options = options.Value ?? throw new ArgumentNullException(nameof(options));

            Console.WriteLine($"DEBUG: JWT Key from config: {_options.Key}");

            if (string.IsNullOrWhiteSpace(_options.Key) || _options.Key.Length < 32)
            {
                throw new ArgumentException("JWT key must be at least 256 bits (32 characters).", nameof(_options.Key));
            }

            if (string.IsNullOrWhiteSpace(_options.Issuer))
            {
                throw new ArgumentException("JWT issuer must be provided.", nameof(_options.Issuer));
            }
        }

        public string GenerateToken(User user)
        {
            try
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Issuer,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating token: {ex}");
                throw new InvalidOperationException("An error occurred while generating the JWT.", ex);
            }
        }
    }
}

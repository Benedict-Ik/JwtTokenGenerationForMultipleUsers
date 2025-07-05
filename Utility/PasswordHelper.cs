namespace JwtTokenGenerationForMultipleUsers.Utility
{
    public class PasswordHelper
    {
        /* Secure password hashing and verification methods are commented out
           for demonstration purposes. In production, consider using a library like BCrypt or Argon2.
           Uncomment and implement these methods as needed.
        */
        //public static string HashPassword(string password)
        //{
        //    // Use a secure hashing algorithm like SHA256 or bcrypt in production
        //    using (var sha256 = System.Security.Cryptography.SHA256.Create())
        //    {
        //        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        //        var hash = sha256.ComputeHash(bytes);
        //        return Convert.ToBase64String(hash);
        //    }
        //}
        //public static bool VerifyPassword(string password, string hashedPassword)
        //{
        //    var hashedInputPassword = HashPassword(password);
        //    return hashedInputPassword == hashedPassword;
        //}
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}

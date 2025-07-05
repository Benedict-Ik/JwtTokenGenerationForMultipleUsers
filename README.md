# Jwt Token Generation For Multiple Users

## Modified `User` Model Class
Added a `PasswordHash` property to store the hashed password.
```C#
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } 
    public string PasswordHash { get; set; } 
}
```

## Ran Migrations

## Installing Bcrypt.Net Package
To handle password hashing, the `Bcrypt.Net-Next` package was installed via the NuGet Package Manager Console using the following command:

## Creating a `PasswordHelper` Class
Created a `PasswordHelper` class within a newly created `Utility` folder to handle password hashing and verification.
```C#
public class PasswordHelper
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
```

## Creating a `UserService` Class
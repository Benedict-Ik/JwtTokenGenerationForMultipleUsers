# Jwt Token Generation For Multiple Users

# UserService

The `UserService` class provides core functionality for managing user records within an application. It acts as a service layer between the database context (`AppDbContext`) and higher-level components like controllers or authentication handlers. This class handles user-related operations such as registration, authentication, retrieval, and deletion.

---

## Responsibilities

- **Authenticate Users**: Securely verify user credentials using hashed password matching.
- **Create Users**: Hash user passwords using BCrypt before saving to the database.
- **Retrieve Users**: Support fetching all registered users.
- **Delete Users**: Allow removal of users by their unique ID.
- **Error Handling**: Wraps all operations in `try...catch` blocks for safe exception management.

---

## Core Methods

### `Task<User?> GetUserAsync(string username, string password)`
Authenticates a user by matching the provided username and plaintext password against stored hashed credentials.

- Returns the user if authentication succeeds
- Returns `null` if the user doesn't exist or password is invalid

---

### `Task<IEnumerable<User>> GetAllUsersAsync()`
Retrieves all registered users from the database.

- Returns a list of `User` objects
- Returns an empty list if an error occurs

---

### `Task<User?> CreateUserAsync(User user, string rawPassword)`
Creates a new user, ensuring the password is securely hashed using BCrypt before saving.

- Returns the newly created user
- Returns `null` if saving fails

---

### `Task<bool> DeleteUserAsync(int id)`
Deletes a user based on their ID.

- Returns `true` if the user is successfully deleted
- Returns `false` if the user does not exist or deletion fails

---

## Security

- Passwords are hashed using `BCrypt.Net` (via `PasswordHelper`) before storage.
- Password comparisons during authentication are made against hashed values.

---

## Dependencies

- `AppDbContext`: Entity Framework Core database context.
- `PasswordHelper`: Utility class responsible for hashing and verifying passwords.

---

## Managing Secrets with Secret Manager
To securely handle sensitive data like JWT keys, database credentials, or API tokens during development, this project uses the .NET Secret Manager in combination with IOptions\<T> binding for clean and type-safe configuration.

### Why Use Secret Manager?
- Keeps secrets out of appsettings.json and source control
- Provides environment-specific storage for local development
- Integrates seamlessly with the .NET configuration system

### Setup Steps
Instead of accessing secrets like config["Jwt:Key"], this project uses a structured options class (JwtSettings) for better maintainability and safety:

**Step 1: Create a strongly typed configuration class**
```C#
public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
}
```

**Step 2: Bind configuration using IOptions\<T> in Program.cs**
```C#
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
```

**Step 3: Store the secrets securely using the Secret Manager CLI**
```Bash
dotnet user-secrets set "Jwt:Key" "9s64e5ba0w325bdb106e2b12r848f7d87c12f00c6c6ed01870579f3613437ad8"
dotnet user-secrets set "Jwt:Issuer" "https://localhost:7234"
dotnet user-secrets set "Jwt:Audience" "https://localhost:7234"
```
>The above secrets are subjective to the user and their system.

**Step 4: Bind configuration settings with the model class**
```C#
public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
}
```
> Properties defined here should match the properties defined in appsettings.

**Case sample:**
```Json
"JwtSettings": {
    "Key": "Key",
    "Issuer": "Issuer",
    "Audience": "Audience",
    "ExpirationInMinutes": 15
}
```
>Placeholders are used above since these values will be overidden by those defined in the Secret Manager.

**Step 5: Use the bound options in your service via dependency injection**
```C#
public class JwtService
{
    private readonly JwtSettings _options;

    public JwtService(IOptions<JwtSettings> options)
    {
        _options = options.Value ;

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
```
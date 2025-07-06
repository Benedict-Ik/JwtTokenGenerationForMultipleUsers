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
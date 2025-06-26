# Jwt Token Generation For Multiple Users

## Defining Simple `User` Model
Created a new cllass called `User` within a newly created folder called `Models`.
```C#
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Store hashed passwords in production
}
```

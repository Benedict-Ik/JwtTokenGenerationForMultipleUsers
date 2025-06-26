# Jwt Token Generation For Multiple Users

## Installing Packages
The below packages have to be installed to use EF Core
- **Microsoft.EntityFrameworkCore**: Provides the core functionality for EF Core.
- **Microsoft.EntityFrameworkCore.SqlServer**: Provides the SQL Server database provider for EF Core.
- **Microsoft.EntityFrameworkCore.Tools**: Provides tools for EF Core, such as migrations and scaffolding.
>Ensure the versions installed are of the same version with the .NET framework being used. For instance, if you have .NET 8 installed, ensure you install packages within 8.x.x.

## Creating DbContext class
Created a new class called `AppDbContext` embedded within a newly created `Data` folder.
```C#
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
}
```
>Note that if there are other entities involved with interwoven relationships, you will have to define the relationships by calling `OnModelCreating()` method.

## Registering newly created DbContext in Program.cs
```C#
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

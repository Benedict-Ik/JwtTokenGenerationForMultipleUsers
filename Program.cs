using JwtTokenGenerationForMultipleUsers.Data;
using JwtTokenGenerationForMultipleUsers.Interfaces;
using JwtTokenGenerationForMultipleUsers.Models;
using JwtTokenGenerationForMultipleUsers.Services;
using JwtTokenGenerationForMultipleUsers.Utility;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Loading Secrets into Configuration
builder.Configuration.AddUserSecrets<Program>();

// Binding JwtSettings from configuration
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddSingleton<PasswordHelper>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using backend.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllers();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

if (!db.Users.Any())
{
    db.Users.Add(new User
    {
        Email = "alice@example.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123")
    });

    db.Users.Add(new User
    {
        Email = "bob@example.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Secret456")
    });

    db.SaveChanges();
}

builder.Services.AddScoped<UsersService>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
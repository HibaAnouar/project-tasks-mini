using backend.Data;
using backend.Models;
using BCrypt.Net;

namespace backend.Services;

public class UsersService
{
    private readonly AppDbContext _db;

    public UsersService(AppDbContext db)
    {
        _db = db;
    }

    public User? GetByEmail(string email) =>
        _db.Users.FirstOrDefault(u => u.Email == email);

    public bool VerifyPassword(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.Verify(password, passwordHash);

    public User CreateUser(string email, string password)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Email = email,
            PasswordHash = hashed
        };

        _db.Users.Add(user);
        _db.SaveChanges();
        return user;
    }
}

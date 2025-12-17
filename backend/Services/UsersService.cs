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

    public User? GetByEmail(string email)
    {
        return _db.Users.FirstOrDefault(u => u.Email == email);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

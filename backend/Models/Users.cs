namespace backend.Models;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    // Store hashed password later
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}

using System;

namespace API.Entities;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString().ToUpper();
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }

    // public DateTime Created { get; set; } = DateTime.UtcNow;
    // public DateTime LastActive { get; set; } = DateTime.UtcNow;
}
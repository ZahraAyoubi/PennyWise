using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Phone { get; set; }
}

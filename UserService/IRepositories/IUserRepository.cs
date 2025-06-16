using UserService.Models;

namespace UserService.IRepositories;

public interface IUserRepository
{
    Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);  
    Task<List<ApplicationUser>> GetAsync();
    Task<ApplicationUser> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<ApplicationUser> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<string> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default);
}

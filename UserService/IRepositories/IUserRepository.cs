using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using UserService.Models;

namespace UserService.IRepositories;

public interface IUserRepository
{
    Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);  
    Task<List<ApplicationUser>> GetAsync();
    Task<ApplicationUser> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<ApplicationUser> LoginAsync(LoginRequest request);
    Task<string> GenerateResetTokenAsync(ApplicationUser user);
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
}
